using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    float randomTime;

    public Transform left;
    public Transform right;

    public float HitLeft;
    public float HitRight;

    private void Start()
    {
        WhenSpawn();
    }

    /// <summary>
    /// 외부에서 RandomTimeSpawn() 코루틴을 접근 하기 위한 함수
    /// </summary>
    public void SummonEnemy()
    {
        StartCoroutine(RandomTimeSpawn());
    }

    /// <summary>
    /// 스폰 시간을 랜덤으로 주고 기다리다 두더지를 스폰 시키는 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomTimeSpawn()
    {
        randomTime = Random.Range(2.0f, 8.0f);
        yield return new WaitForSeconds(randomTime);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    /// <summary>
    /// 1000좌표에서 바닥까지 떨어지는 함수
    /// </summary>
    private void SetGravity()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //바닥에 닿으면
            if (hit.collider.gameObject.CompareTag("Bottom"))
            {
            //그곳으로 이동
               transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
               //transform.rotation = Quaternion.Euler(hit.normal.x, hit.normal.y, transform.rotation.z);
            }
        }
    }

    private void WhenSpawn()
    {
        RaycastHit leftHit;
        Ray leftRay = new Ray(left.position, Vector3.down);

        RaycastHit rightHit;
        Ray rightRay = new Ray(right.position, Vector3.down);

        if (Physics.Raycast(leftRay, out leftHit, Mathf.Infinity))
        {
            if (Physics.Raycast(rightRay, out rightHit, Mathf.Infinity))
            {
                if(leftHit.collider.gameObject.CompareTag("Bottom") && rightHit.collider.gameObject.CompareTag("Bottom"))
                {
                    HitLeft = leftHit.point.y;
                    HitRight = rightHit.point.y;
                    if (Mathf.Abs(leftHit.point.y - rightHit.point.y) > 2)
                    {
                        transform.position = GameManager.Instance.randomTransformSpawn();
                        WhenSpawn();
                    }
                    else
                    {
                        SetGravity();
                        return;
                    }
                }
            }
        }
    }
}
