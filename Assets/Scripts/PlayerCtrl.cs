using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("�Ӽ�")]
    //1. ĳ���� �̵��ӵ� ����
    [Tooltip("ĳ���� �̵��ӵ� ����")]
    public float movedSpd = 2.0f;

    //2. ĳ���� �̵��ӵ� ����(�޸���)
    public float runMoveSpd = 3.5f;

    //3. ĳ���� �̵����� / ȸ�� �ӵ� ����
    public float DirectionRotateSpd = 100.0f;

    //4. ĳ���� ���� �����̴� ȸ�� �ӵ� ����
    public float BodyRotateSpd = 2.0f;

    //5. ĳ���� �ӵ� ���� ���� ��
    [Range(0.1f, 50.0f)]
    public float VelocityChangeSpd = 0.1f;

    //6. ĳ���� ���� �̵� �ӵ� ���� �ʱⰪ
    private Vector3 CurrentVelocitySpd = Vector3.zero;

    //7. ĳ���� ���� �̵� ���� �ʱⰪ ����
    private Vector3 MoveDirection = Vector3.zero;

    //8. CharacterController ĳ�� �غ�
    private CharacterController characterCtrl = null;

    //9. �浹ü ���� FLAG
    private CollisionFlags collisionFlags = CollisionFlags.None;

    //�߷°�
    private float gravity = 9.8f;

    //���� ĳ���� �ӵ�
    private float verticalSpd = 0f;

    //ĳ���� ���� Flag
    private bool stopPlayer = false;


    //animation component ĳ�� �غ�
    private Animator animationPlayer = null;

    //ĳ���� ����
    public enum PlayerState { None, Idle, Walk, Run, Atk, Skill }

    [Header("ĳ���� ����")]
    public PlayerState playerState = PlayerState.None;

    //���� ����
    public enum PlayerAttackState { atkStep_1, atkStep_2, atkStep_3, atkStep_4 }

    [Header("���� ����")]
    public PlayerAttackState playerAttackState = PlayerAttackState.atkStep_1;

    //���� ���� ���� Ȱ��ȭ ���θ� Ȯ�� �ϱ� ���� flag ������
    public bool flagNextAttack = false;

    [Header("���� ����")]
    //���� �� ���� ������
    public TrailRenderer AtkTrailRenderer = null;

    //���⿡ �ִ� �ݶ��̴� ĳ��
    public CapsuleCollider AtkCapsuleCollider = null;

    [Header("��ų ����")]
    public GameObject skillEffect = null;


    void Start()
    {
        //CharacterController ĳ��
        characterCtrl = GetComponent<CharacterController>();

        //animation ĳ��
        animationPlayer = GetComponent<Animator>();
        //���� ���� �⺻��
        playerState = PlayerState.Idle;
    }

    void Update()
    {
        //ĳ���� �̵�
        Move();

        //�߷� ����
        SetGravity();

        //�÷��̾� ȸ��
        BodyDirectionChange();

        //state�� ���� �ִϸ��̼� ���
        AnimationClipCtrl();

        //���ǿ� ���缭 �÷��̾� ���� ����
        CheckAnimationState();

        //���� ���� Ȯ��
        InputAttackCtrl();
    }

    /// <summary>
    /// ĳ���� �̵� �Լ�
    /// </summary>
    void Move()
    {
        //�÷��̾� stop ����
        if (stopPlayer == true)
        {
            return;
        }

        //���� ī�޶� Transform
        Transform cameraTransform = Camera.main.transform;

        //���� ī�޶� �ٶ󺸴� ������ ����󿡼� � �����ΰ�
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //���� ����
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //Ű ��
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //���� �������� ��ǥ��
        Vector3 targetDirection = vertical * forward + horizontal * right;

        //�̵� ����
        MoveDirection = Vector3.RotateTowards(MoveDirection, targetDirection, DirectionRotateSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        MoveDirection = MoveDirection.normalized;

        //�̵� �ӵ�
        float spd = movedSpd;

        //���࿡ playerState�� Run�̸� 
        if (playerState == PlayerState.Run)
        {
            spd = runMoveSpd;
        }
        else if (playerState == PlayerState.Walk)
        {
            spd = movedSpd;
        }
        Vector3 vecGravity = new Vector3(0f, verticalSpd, 0f);

        //�̵��ϴ� ������ ��
        Vector3 moveAmount = (MoveDirection * spd * Time.deltaTime) + vecGravity;

        //���� �̵�
        collisionFlags = characterCtrl.Move(moveAmount);

    }

    /// <summary>
    /// �ӵ��� ��ȯ�ϴ� �Լ�
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
    /// �����̴� ������ ���� ���� �Լ� �ۼ�
    /// </summary>
    void BodyDirectionChange()
    {
        //�����̰� �ִ°�?
        if (GetVelocitySpd() > 0.0f)
        {
            //ĳ���� ������ �ٶ󺸴� ������? ĳ���� �ӵ� ����
            Vector3 newForward = characterCtrl.velocity;
            newForward.y = 0.0f;

            //ĳ���͸� �������� ������ �����Ѵ�.
            transform.forward = Vector3.Lerp(transform.forward, newForward, BodyRotateSpd * Time.deltaTime);

        }
    }

    /// <summary>
    /// ȭ�鿡 �۾��� ����ִ� �Լ�
    /// </summary>
    private void OnGUI()
    {
        if (characterCtrl != null && characterCtrl.velocity != Vector3.zero)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 15;
            labelStyle.normal.textColor = Color.black;

            //���� �ӵ� 
            float _getVelocity = GetVelocitySpd();
            GUILayout.Label("���� �ӵ� : " + _getVelocity.ToString(), labelStyle);

            //���� ĳ���� ����
            GUILayout.Label("���� ���� : " + characterCtrl.velocity.ToString(), labelStyle);

            //���� ĳ���� �ӵ�
            GUILayout.Label("���� ĳ���� �ӵ� : " + CurrentVelocitySpd.magnitude.ToString(), labelStyle);

            GUILayout.Label("�浹 : " + collisionFlags.ToString(), labelStyle);
        }
    }

    /// <summary>
    /// ���� ��ư
    /// </summary>
    void InputAttackCtrl()
    {
        //���콺 Ŭ�� �Ͽ��°�?
        if (Input.GetMouseButtonDown(0) == true)
        {
            //�÷��̾� ���� ����
            if (playerState != PlayerState.Atk)
            {
                //�÷��̾ ���� ���°� �ƴϸ� ���� ���·� ����
                playerState = PlayerState.Atk;

                //���ݻ��� �ʱ�ȭ
                playerAttackState = PlayerAttackState.atkStep_1;
            }
            else
            {
                //�÷��̾� ���°� ���ݻ��� 
                //���� ���¿� ���� �з�
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

        //���콺 ��Ŭ��
        if (Input.GetMouseButtonDown(1) == true)
        {
            //�÷��̾� ���� ����
            if (playerState != PlayerState.Skill)
            {
                //�÷��̾ ���� ���°� �ƴϸ� ���� ���·� ����
                playerState = PlayerState.Skill;
            }
        }
    }

    /// <summary>
    /// CallBack ���� �ִϸ��̼� ����� ������ ȣ�� �Ǵ� �ִϸ��̼� �̺�Ʈ �Լ�
    /// </summary>
    void OnPlayerAttackFinshed()
    {
        //���࿡ flagNextAttack�� true�� 
        if (flagNextAttack == true)
        {
            //flag �ʱ�ȭ
            flagNextAttack = false;



            Debug.Log("a");
            //���� ���� �ִϸ��̼� ���¿� ���� ���� �ִϸ��̼� ���°��� �ֱ�
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
    /// �߷����� �Լ�
    /// </summary>
    void SetGravity()
    {
        //ĳ���Ͱ� �ٴڿ� �پ��ٸ�

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
    /// �÷��̾� ���¿� ���缭 �ִϸ��̼��� ���
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
    /// ���¿� ���� �������ִ� �Լ�
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
    /// ���� �ִϸ��̼� ���
    /// </summary>
    void AtkAnimationCtrl()
    {

        //���� ���� ���°�?
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
