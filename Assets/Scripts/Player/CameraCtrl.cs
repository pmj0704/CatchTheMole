using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("카메라 기본 속성")]
    //카메라 위치 캐싱 준비
    private Transform cameraTransform = null;

    //target
    public GameObject objTarget = null;

    //player transform 캐싱
    private Transform objTargetTransform = null;

    public Transform camTrans;

    public LayerMask layerMask;


    [Header("3인칭 카메라")]
    //떨어진 거리
    public float distance = 6.0f;

    //추가 높이
    public float height = 1.75f;
    float originalDis = 0f;
    float originalHeight = 0f;

    //smooth time
    public float heightDamp = 2f;
    public float rotationDamping = 3f;

    public bool inHouse = false;


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
        originalDis = distance;
        originalHeight = height;

        //타겟이 있는가?
        if (objTarget != null)
        {
            objTargetTransform = objTarget.transform;
        }
    }


    /// <summary>
    /// 3인칭 카메라 기본 동작 함수
    /// </summary>
    private void ThridCamera()
    {
        //현재 타겟 Y축 각도 값
        float objTargetRotationAngle = objTargetTransform.eulerAngles.y;

        //현재 타겟 높이 + 카메라가 위치한 높이 추가 높이
        float objHeight = objTargetTransform.position.y + height;
        //현재 각도 높이
        float nowRotationAngle = cameraTransform.eulerAngles.y;
        float nowHeight = cameraTransform.position.y;
        //현재 각도에 대한 Damp
        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, rotationDamping * Time.deltaTime);
        //현재 높이에 대한 Damp
        nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamp * Time.deltaTime);

        //유니티 각도로 변경
        Quaternion nowRotation = Quaternion.Euler(0f, nowRotationAngle, 0f);

        //카메라 위치 포지션 이동
        cameraTransform.position = objTargetTransform.position;
        cameraTransform.position -= nowRotation * Vector3.forward * distance;

        RaycastHit hit;
        Ray ray = new Ray(camTrans.position, transform.position);

        if (!inHouse)
        {
            if (Physics.Raycast(ray, out hit, originalDis, layerMask))
            {
                distance = Vector3.Distance(hit.point, camTrans.position);
                height = Vector3.Distance(hit.point, camTrans.position) / 3;
            }
            else
            {
                distance = originalDis;
                height = originalHeight;
            }
        }
        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeight, cameraTransform.position.z);

        cameraTransform.LookAt(objTargetTransform);
    }
}
