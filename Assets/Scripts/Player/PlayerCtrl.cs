using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("속성")]
    //1. 캐릭터 이동속도 설정
    [Tooltip("캐릭터 이동속도 설정")]
    public float movedSpd = 2.0f;

    //2. 캐릭터 이동속도 설정(달리기)
    public float runMoveSpd = 3.5f;

    //3. 캐릭터 이동방향 / 회전 속도 설정
    public float DirectionRotateSpd = 100.0f;

    //4. 캐릭터 몸을 움직이는 회전 속도 설정
    public float BodyRotateSpd = 2.0f;

    //5. 캐릭터 속도 변경 증가 값
    [Range(0.1f, 50.0f)]
    public float VelocityChangeSpd = 0.1f;

    //6. 캐릭터 현재 이동 속도 설정 초기값
    private Vector3 CurrentVelocitySpd = Vector3.zero;

    //7. 캐릭터 현재 이동 방향 초기값 설정
    private Vector3 MoveDirection = Vector3.zero;

    //8. CharacterController 캐싱 준비
    private CharacterController characterCtrl = null;

    //9. 충돌체 받을 FLAG
    private CollisionFlags collisionFlags = CollisionFlags.None;

    //중력값
    private float gravity = 9.8f;

    //현재 캐릭터 속도
    private float verticalSpd = 0f;

    //캐릭터 정지 Flag
    private bool stopPlayer = false;

    

    //animation component 캐싱 준비
    private Animator animationPlayer = null;

    //캐릭터 상태
    public enum PlayerState { None, Idle, Walk, Run, Atk, Skill }

    [Header("캐릭터 상태")]
    public PlayerState playerState = PlayerState.None;

    //공격 상태
    public enum PlayerAttackState { atkStep_1, atkStep_2, atkStep_3, atkStep_4 }

    [Header("공격 상태")]
    public PlayerAttackState playerAttackState = PlayerAttackState.atkStep_1;

    //다음 연계 공격 활성화 여부를 확인 하기 위해 flag 서렂ㅇ
    public bool flagNextAttack = false;

    [Header("전투 관련")]

    //무기에 있는 콜라이더 캐싱
    public CapsuleCollider AtkCapsuleCollider = null;

    [Header("애니메이션 클립")]
    public AnimationClip IdleClip = null;
    public AnimationClip WalkClip = null;
    public AnimationClip RunClip = null;
    public AnimationClip AtkClip1 = null;
    //public AnimationClip AtkClip2 = null;
    //public AnimationClip AtkClip3 = null;
    //public AnimationClip AtkClip4 = null;

    [Header("Ray")]
    public Transform rayStart;
    public int rayDis = 5;
    public GameObject FText;

    [Header("문 상호 작용")]
    public GameObject[] doorTxt;

    public float FeverLength = 10f;

    public GameObject trail;

    void Start()
    {
        //CharacterController 캐싱
        characterCtrl = GetComponent<CharacterController>();

        //animation 캐싱
        animationPlayer = GetComponent<Animator>();
        //상태 값을 기본값
        playerState = PlayerState.Idle;

        AtkClip1.wrapMode = WrapMode.Once;
        //AtkClip2.wrapMode = WrapMode.Once;
        //AtkClip3.wrapMode = WrapMode.Once;
        //AtkClip4.wrapMode = WrapMode.Once;
    }

    void Update()
    {
        if (GameManager.Instance.isUI == false && GameManager.Instance.isEsc == false)
        {
            //캐릭터 이동
            Move();

            //중력 적용
            SetGravity();

            //플레이어 회전
            BodyDirectionChange();

            //state에 따른 애니메이션 재생
            AnimationClipCtrl();

            //조건에 맞춰서 플레이어 상태 변경
            CheckAnimationState();

            //공격 여부 확인
            InputAttackCtrl();

            //앞 물체 확인
            CheckFront();

            //피버 상태인지 확인
            Fever();

        }
        else
        {
            playerState = PlayerState.Idle;
            CheckAnimationState();
        }
    }

    /// <summary>
    /// 캐릭터 이동 함수
    /// </summary>
    void Move()
    {
        //플레이어 stop 여부
        if (stopPlayer == true)
        {
            return;
        }

        //메인 카메라 Transform
        Transform cameraTransform = Camera.main.transform;

        //메인 카메라가 바라보는 방향이 월드상에서 어떤 방향인가
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //벡터 내적
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //키 값
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //방향 벡터이자 목표점
        Vector3 targetDirection = vertical * forward + horizontal * right;

        //이동 방향
        MoveDirection = Vector3.RotateTowards(MoveDirection, targetDirection, DirectionRotateSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        MoveDirection = MoveDirection.normalized;

        //이동 속도
        float spd = movedSpd;

        //만약에 playerState가 Run이면 
        if (playerState == PlayerState.Run)
        {
            spd = runMoveSpd;
        }
        else if (playerState == PlayerState.Walk)
        {
            spd = movedSpd;
        }
        Vector3 vecGravity = new Vector3(0f, verticalSpd, 0f);

        //이동하는 프레임 양
        Vector3 moveAmount = (MoveDirection * spd * Time.deltaTime) + vecGravity;

        //실제 이동
        collisionFlags = characterCtrl.Move(moveAmount);

    }

    /// <summary>
    /// 속도를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    float GetVelocitySpd()
    {
        if (characterCtrl.velocity == Vector3.zero)
        {
            CurrentVelocitySpd = Vector3.zero;
        }
        else
        {
            Vector3 retVelocitySpd = characterCtrl.velocity;
            retVelocitySpd.y = 0;
            CurrentVelocitySpd = Vector3.Lerp(CurrentVelocitySpd, retVelocitySpd, VelocityChangeSpd * Time.fixedDeltaTime);
        }
        return CurrentVelocitySpd.magnitude;
    }

    /// <summary>
    /// 움직이는 방향을 몸통 방향 함수 작성
    /// </summary>
    void BodyDirectionChange()
    {
        //움직이고 있는가?
        if (GetVelocitySpd() > 0.0f)
        {
            //캐릭터 몸통이 바라보는 전장은? 캐릭터 속도 방향
            Vector3 newForward = characterCtrl.velocity;
            newForward.y = 0.0f;

            //캐릭터를 전방으로 방향을 설정한다.
            transform.forward = Vector3.Lerp(transform.forward, newForward, BodyRotateSpd * Time.deltaTime);

        }
    }
    
    /// <summary>
    /// 공격 버튼
    /// </summary>
    void InputAttackCtrl()
    {
        //마우스 클릭 하였는가?
        if (Input.GetMouseButtonDown(0) == true && !GameManager.Instance.inHouse)
        {
            //플레이어 공격 상태
            if (playerState != PlayerState.Atk)
            {
                GameManager.Instance.PlayerAtk();

                //플레이어가 공격 상태가 아니면 공격 상태로 변경
                playerState = PlayerState.Atk;

                //공격상태 초기화
                playerAttackState = PlayerAttackState.atkStep_1;
            }
            //else
            //{
            //    //플레이어 상태가 공격상태 
            //    //공격 상태에 따른 분류
            //    switch (playerAttackState)
            //    {
            //        case PlayerAttackState.atkStep_1:
            //            if (animationPlayer.GetCurrentAnimatorStateInfo(0).IsName(AtkClip1.name) && animationPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f)
            //            {
            //                flagNextAttack = true;
            //            }
            //            break;
            //        case PlayerAttackState.atkStep_2:
            //            if (animationPlayer.GetCurrentAnimatorStateInfo(0).IsName(AtkClip2.name) && animationPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f)
            //            {
            //                flagNextAttack = true;
            //            }
            //            break;
            //        case PlayerAttackState.atkStep_3:
            //            if (animationPlayer.GetCurrentAnimatorStateInfo(0).IsName(AtkClip3.name) && animationPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f)
            //            {
            //                flagNextAttack = true;
            //            }
            //            break;
            //        case PlayerAttackState.atkStep_4:
            //            if (animationPlayer.GetCurrentAnimatorStateInfo(0).IsName(AtkClip4.name) && animationPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f)
            //            {
            //                flagNextAttack = true;
            //            }
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        ////마우스 우클릭
        //if (Input.GetMouseButtonDown(1) == true)
        //{
        //    //플레이어 공격 상태
        //    if (playerState != PlayerState.Skill)
        //    {
        //        //플레이어가 공격 상태가 아니면 공격 상태로 변경
        //        playerState = PlayerState.Skill;
        //    }
        //}
    }

    /// <summary>
    /// CallBack 공격 애니메이션 재생이 끝나면 호출 되는 애니메이션 이벤트 함수
    /// </summary>
    public void OnPlayerAttackFinshed()
    {
            stopPlayer = false;
            playerState = PlayerState.Idle;

            playerAttackState = PlayerAttackState.atkStep_1;
    }

    /// <summary>
    /// 중력적용 함수
    /// </summary>
    void SetGravity()
    {
        //캐릭터가 바닥에 붙었다면

        if ((collisionFlags & CollisionFlags.CollidedBelow) != 0)
        {
            verticalSpd = 0f;
        }
        else
        {
            verticalSpd -= gravity * Time.deltaTime;
        }
    }

    /// <summary>
    /// 앞에 어떤 물체가 있는 확인하는 함수
    /// </summary>
    void CheckFront()
    {
        RaycastHit hit;
        Ray ray = new Ray(rayStart.position, transform.forward);

        if(Physics.Raycast(ray, out hit, rayDis))
        {
            if (hit.collider.CompareTag("Door"))
            {
                doorTxt[0].SetActive(true);
                doorTxt[1].SetActive(true);

                if(Input.GetKeyDown(KeyCode.F))
                {
                    hit.collider.gameObject.GetComponent<Door>().Interact(true);
                }
            }
            else
            {
                doorTxt[0].SetActive(false);
                doorTxt[1].SetActive(false);
            }

            if (hit.collider.CompareTag("Jack"))
            {
                hit.collider.gameObject.GetComponent<Jack>().Interact(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.Instance.OpenStore();
                }
            }
            else
            {
                if (hit.collider.gameObject.GetComponent<Jack>() != null)
                hit.collider.gameObject.GetComponent<Jack>().Interact(false);
                FText.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 클립을 플레이 해주는 함수
    /// </summary>
    /// <param name="clip"></param>
    void AnimationClipPlay(AnimationClip clip)
    {
        animationPlayer.Play(clip.name);
    }

    /// <summary>
    /// 플레이어 상태에 맞춰서 애니메이션을 재생
    /// </summary>
    void AnimationClipCtrl()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                AnimationClipPlay(IdleClip);
                break;
            case PlayerState.Walk:
                AnimationClipPlay(WalkClip);
                break;
            case PlayerState.Run:
                AnimationClipPlay(RunClip);
                break;
            case PlayerState.Atk:
                AnimationClipPlay(AtkClip1);

                break;
            case PlayerState.Skill:
                animationPlayer.Play("");
                break;
        }
    }

    /// <summary>
    /// 상태에 따른 변경해주는 함수
    /// </summary>
    void CheckAnimationState()
    {
        float nowSdp = GetVelocitySpd();
        switch (playerState)
        {
            case PlayerState.Idle:
                AtkCapsuleCollider.enabled = false;
                if (nowSdp > 0.0f)
                {
                    playerState = PlayerState.Walk;
                }
                break;
            case PlayerState.Walk:
                AtkCapsuleCollider.enabled = false;
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    playerState = PlayerState.Run;
                }
                else if (nowSdp < 0.1f)
                {
                    playerState = PlayerState.Idle;
                }
                else if(GameManager.Instance.isUI)
                {
                    playerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Run:
                AtkCapsuleCollider.enabled = false;
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    playerState = PlayerState.Walk;
                }

                if (nowSdp < 0.1f)
                {
                    playerState = PlayerState.Idle;
                }
                else if (GameManager.Instance.inHouse)
                {
                    playerState = PlayerState.Walk;
                }
                else if (GameManager.Instance.isUI)
                {
                    playerState = PlayerState.Idle;
                }

                break;
            case PlayerState.Atk:
                AtkCapsuleCollider.enabled = true;
                stopPlayer = true;
                //AtkAnimationCtrl();
                AnimationClipPlay(AtkClip1);
                break;
            case PlayerState.Skill:
                break;
        }
    }
    ///// <summary>
    ///// 공격 애니메이션 재생
    ///// </summary>
    //void AtkAnimationCtrl()
    //{
    //    //만약 공격 상태가?
    //    switch (playerAttackState)
    //    {
    //        case PlayerAttackState.atkStep_1:
    //            AnimationClipPlay(AtkClip1);
    //            break;
    //        case PlayerAttackState.atkStep_2:
    //            AnimationClipPlay(AtkClip2);
    //            break;
    //        case PlayerAttackState.atkStep_3:
    //            AnimationClipPlay(AtkClip3);
    //            break;
    //        case PlayerAttackState.atkStep_4:
    //            AnimationClipPlay(AtkClip4);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    /// <summary>
    /// 공격 시 망치 콜라이더
    /// </summary>
    /// <param name="stopPlayer"></param>
    public void HammerCol(bool stopPlayer)
    {
        this.stopPlayer = stopPlayer;
        characterCtrl.Move(-transform.forward * 0.05f);
    }

    /// <summary>
    /// 피버 시간이면 시작
    /// </summary>
    void Fever()
    {
        if(GameManager.Instance.isFeverTime)
        {
            StartCoroutine(FeverTime());
        }
    }

    /// <summary>
    /// 피버 시간 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FeverTime()
    {
        GameManager.Instance.isFeverTime = false;
        GameManager.Instance.atFever();
        movedSpd *= 3;
        runMoveSpd *=  3;

        trail.SetActive(true);
        yield return new WaitForSeconds(FeverLength);

        movedSpd /=  3;
        runMoveSpd /=  3;

        GameManager.Instance.BlackFeverUI();
        trail.SetActive(false);
        GameManager.Instance.SummonFever();
        GameManager.Instance.FeverEnd();
        GameManager.Instance.isFever = false;
    }
}
