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
    //공격 할 때만 켜지게
    public TrailRenderer AtkTrailRenderer = null;

    //무기에 있는 콜라이더 캐싱
    public CapsuleCollider AtkCapsuleCollider = null;

    [Header("스킬 관련")]
    public GameObject skillEffect = null;


    void Start()
    {
        //CharacterController 캐싱
        characterCtrl = GetComponent<CharacterController>();

        //animation 캐싱
        animationPlayer = GetComponent<Animator>();
        //상태 값을 기본값
        playerState = PlayerState.Idle;
    }

    void Update()
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
    /// 화면에 글씨를 띄어주는 함수
    /// </summary>
    private void OnGUI()
    {
        if (characterCtrl != null && characterCtrl.velocity != Vector3.zero)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 15;
            labelStyle.normal.textColor = Color.black;

            //현재 속도 
            float _getVelocity = GetVelocitySpd();
            GUILayout.Label("현재 속도 : " + _getVelocity.ToString(), labelStyle);

            //현재 캐릭터 방향
            GUILayout.Label("현재 방향 : " + characterCtrl.velocity.ToString(), labelStyle);

            //현재 캐릭터 속도
            GUILayout.Label("현재 캐릭터 속도 : " + CurrentVelocitySpd.magnitude.ToString(), labelStyle);

            GUILayout.Label("충돌 : " + collisionFlags.ToString(), labelStyle);
        }
    }

    /// <summary>
    /// 공격 버튼
    /// </summary>
    void InputAttackCtrl()
    {
        //마우스 클릭 하였는가?
        if (Input.GetMouseButtonDown(0) == true)
        {
            //플레이어 공격 상태
            if (playerState != PlayerState.Atk)
            {
                //플레이어가 공격 상태가 아니면 공격 상태로 변경
                playerState = PlayerState.Atk;

                //공격상태 초기화
                playerAttackState = PlayerAttackState.atkStep_1;
            }
            else
            {
                //플레이어 상태가 공격상태 
                //공격 상태에 따른 분류
                switch (playerAttackState)
                {
                    case PlayerAttackState.atkStep_1:
                        break;
                    case PlayerAttackState.atkStep_2:
                        break;
                    case PlayerAttackState.atkStep_3:
                        break;
                    case PlayerAttackState.atkStep_4:
                        break;
                    default:
                        break;
                }
            }
        }

        //마우스 우클릭
        if (Input.GetMouseButtonDown(1) == true)
        {
            //플레이어 공격 상태
            if (playerState != PlayerState.Skill)
            {
                //플레이어가 공격 상태가 아니면 공격 상태로 변경
                playerState = PlayerState.Skill;
            }
        }
    }

    /// <summary>
    /// CallBack 공격 애니메이션 재생이 끝나면 호출 되는 애니메이션 이벤트 함수
    /// </summary>
    void OnPlayerAttackFinshed()
    {
        //만약에 flagNextAttack이 true면 
        if (flagNextAttack == true)
        {
            //flag 초기화
            flagNextAttack = false;



            Debug.Log("a");
            //현재 공격 애니메이션 상태에 따른 다음 애니메이션 상태값을 넣기
            switch (playerAttackState)
            {
                case PlayerAttackState.atkStep_1:
                    playerAttackState = PlayerAttackState.atkStep_2;
                    Debug.Log("1");
                    break;
                case PlayerAttackState.atkStep_2:
                    playerAttackState = PlayerAttackState.atkStep_3;
                    Debug.Log("2");
                    break;
                case PlayerAttackState.atkStep_3:
                    playerAttackState = PlayerAttackState.atkStep_4;
                    Debug.Log("3");
                    break;
                case PlayerAttackState.atkStep_4:
                    playerAttackState = PlayerAttackState.atkStep_1;
                    Debug.Log("4");
                    break;
                default:
                    break;
            }
        }
        else
        {
            stopPlayer = false;
            playerState = PlayerState.Idle;

            playerAttackState = PlayerAttackState.atkStep_1;
        }
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
    /// 플레이어 상태에 맞춰서 애니메이션을 재생
    /// </summary>
    void AnimationClipCtrl()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                animationPlayer.Play("Walking@loop");
                break;
            case PlayerState.Walk:
                animationPlayer.Play("");
                break;
            case PlayerState.Run:
                animationPlayer.Play("");
                break;
            case PlayerState.Atk:
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
                if (nowSdp > 0.0f)
                {
                    playerState = PlayerState.Walk;
                }
                break;
            case PlayerState.Walk:
                if (nowSdp > 2.0f)
                {
                    playerState = PlayerState.Run;
                }
                else if (nowSdp < 0.1f)
                {
                    playerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Run:
                if (nowSdp < 2.0f)
                {
                    playerState = PlayerState.Walk;
                }

                if (nowSdp < 0.1f)
                {
                    playerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Atk:
                stopPlayer = true;
                AtkAnimationCtrl();
                break;
            case PlayerState.Skill:
                break;
        }
    }
    /// <summary>
    /// 공격 애니메이션 재생
    /// </summary>
    void AtkAnimationCtrl()
    {

        //만약 공격 상태가?
        switch (playerAttackState)
        {
            case PlayerAttackState.atkStep_1:
                animationPlayer.Play("");
                break;
            case PlayerAttackState.atkStep_2:
                animationPlayer.Play("");
                break;
            case PlayerAttackState.atkStep_3:
                animationPlayer.Play("");
                break;
            case PlayerAttackState.atkStep_4:
                animationPlayer.Play("");
                break;
            default:
                break;
        }
    }
}
