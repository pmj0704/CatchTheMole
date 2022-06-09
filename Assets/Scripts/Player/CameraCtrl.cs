using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("ī�޶� �⺻ �Ӽ�")]
    //ī�޶� ��ġ ĳ�� �غ�
    private Transform cameraTransform = null;

    //target
    public GameObject objTarget = null;

    //player transform ĳ��
    private Transform objTargetTransform = null;


    [Header("3��Ī ī�޶�")]
    //������ �Ÿ�
    public float distance = 6.0f;

    //�߰� ����
    public float height = 1.75f;

    //smooth time
    public float heightDamp = 2f;
    public float rotationDamping = 3f;
    private void LateUpdate()
    {
        if (objTarget == null)
        {
            return;
        }

        if (objTargetTransform == null)
        {
            objTargetTransform = objTarget.transform;
        }

        ThridCamera();
    }

    void Start()
    {
        cameraTransform = GetComponent<Transform>();

        //Ÿ���� �ִ°�?
        if (objTarget != null)
        {
            objTargetTransform = objTarget.transform;
        }
    }


    /// <summary>
    /// 3��Ī ī�޶� �⺻ ���� �Լ�
    /// </summary>
    private void ThridCamera()
    {
        //���� Ÿ�� Y�� ���� ��
        float objTargetRotationAngle = objTargetTransform.eulerAngles.y;

        //���� Ÿ�� ���� + ī�޶� ��ġ�� ���� �߰� ����
        float objHeight = objTargetTransform.position.y + height;
        //���� ���� ����
        float nowRotationAngle = cameraTransform.eulerAngles.y;
        float nowHeight = cameraTransform.position.y;
        //���� ������ ���� Damp
        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, rotationDamping * Time.deltaTime);
        //���� ���̿� ���� Damp
        nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamp * Time.deltaTime);

        //����Ƽ ������ ����
        Quaternion nowRotation = Quaternion.Euler(0f, nowRotationAngle, 0f);

        //ī�޶� ��ġ ������ �̵�
        cameraTransform.position = objTargetTransform.position;
        cameraTransform.position -= nowRotation * Vector3.forward * distance;

        //���� �̵�
        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeight, cameraTransform.position.z);

        cameraTransform.LookAt(objTargetTransform);
    }
}