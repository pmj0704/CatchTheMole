using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    float randomTime;

    public Transform left;
    public Transform right;

    public GameObject ExMark;

    public float HitLeft;
    public float HitRight;

    public float minSpawnRate = 2.0f;
    public float maxSpawnRate = 8.0f;

    public Transform hitPos;

    public GameManager.Alphabet aplhabet = GameManager.Alphabet.NULL;

    public LayerMask layerMask;
    public LayerMask bottomLayerMask;

    private void Start()
    {
        Respawn();
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
        yield return new WaitForSeconds(1f);
        ExMark.SetActive(false);
        randomTime = Random.Range(minSpawnRate, maxSpawnRate);
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

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if(hit.collider.gameObject.CompareTag("Building"))
            {
                Respawn();
            }
            //바닥에 닿으면
            if (hit.collider.gameObject.CompareTag("Bottom"))
            {
            //그곳으로 이동
               transform.position = new Vector3(transform.position.x, hit.point.y + 0.2f, transform.position.z);
               //transform.rotation = Quaternion.Euler(hit.normal.x, hit.normal.y, transform.rotation.z);
            }
        }
    }

    /// <summary>
    /// 스폰 후 두더지
    /// </summary>
    private void WhenSpawn()
    {
        RaycastHit leftHit;
        Ray leftRay = new Ray(left.position, Vector3.down);

        RaycastHit rightHit;
        Ray rightRay = new Ray(right.position, Vector3.down);

        if (Physics.Raycast(leftRay, out leftHit, Mathf.Infinity, bottomLayerMask))
        {
            if (Physics.Raycast(rightRay, out rightHit, Mathf.Infinity,bottomLayerMask))
            {
                //두더지가 평평한 곳에 소환 되도록 지점을 찾고
                    HitLeft = leftHit.point.y;
                    HitRight = rightHit.point.y;
                    //평평한 곳이 아니면 다시
                    if (Mathf.Abs(leftHit.point.y - rightHit.point.y) > 2)
                    {
                        Respawn();
                    }
                    //아니면 내려감
                    else
                    {
                        SetGravity();
                    return;
                    }
            }
        }
    }

    /// <summary>
    /// 두더지를 재생성 하는 함수
    /// </summary>
    private void Respawn()
    {
        transform.position = GameManager.Instance.randomTransformSpawn();
        WhenSpawn();
    }

    public void OnCollisionEnter(Collision collision)
    {
        //서로 겹치지 않기 위한 함수
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Respawn();
        }
    }
}
