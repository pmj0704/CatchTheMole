using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    //두더지 소환 시간
    float randomTime;
    public float minSpawnRate = 2.0f;
    public float maxSpawnRate = 8.0f;

    //나무 위, 경사면 생성을 방지 하기 위한 왼쪽 오른쪽 레이케스트 시작 점
    public Transform left;
    public Transform right;

    //중력 처리와 생성 버그용 레이 시작점
    public Transform hitPos;

    //레이용 레이어 마스크
    public LayerMask layerMask;
    public LayerMask bottomLayerMask;

    // 두더지 잡으면 나오는 !
    public GameObject ExMark;

    //레이 Hit 위치
    public float HitLeft;
    public float HitRight;
    public float HitBottom;

    //피버 두더지일 때 가지고 있는 알파벳
    public int alphabet = 100;

    //알파벳을 가지고 있는 두더지 인지
    private bool hasAlphabet = false;

    //처음 잡은 두더지인지
    private bool firstTime = true;

    //처음 잡은 두더지에 나오는 이펙트
    public GameObject firstEffect;

    public Renderer HoleRenderer = null;
    public Material afterMet;

    public int index = 100;

    private void Start()
    {
        Respawn();
    }

    /// <summary>
    /// 외부에서 RandomTimeSpawn() 코루틴을 접근 하기 위한 함수
    /// </summary>
    public void SummonEnemy()
    {
        hasAlphabet = false;
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
            if (hit.collider.gameObject.CompareTag("Building"))
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

        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        RaycastHit leftHit;
        Ray leftRay = new Ray(left.position, Vector3.down);

        RaycastHit rightHit;
        Ray rightRay = new Ray(right.position, Vector3.down);

        if (Physics.Raycast(leftRay, out leftHit, Mathf.Infinity, bottomLayerMask))
        {
            if (Physics.Raycast(rightRay, out rightHit, Mathf.Infinity, bottomLayerMask))
            {
                //두더지가 평평한 곳에 소환 되도록 지점을 찾고
                HitLeft = leftHit.point.y;
                HitRight = rightHit.point.y;
                //평평한 곳이 아니면 다시
                if (Mathf.Abs(HitLeft - HitRight) > 2)
                {
                    Respawn();
                }
                //아니면 내려감
                else
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, bottomLayerMask))
                    {
                        HitBottom = hit.point.y;
                        if ((HitLeft - HitBottom) > 2f || (HitRight - HitBottom) > 2f)
                        {
                            Respawn();
                        }
                    }
                }

                SetGravity();
                return;
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

    /// <summary>
    /// 피버 타임을 위한 함수
    /// </summary>
    public void Dead()
    {
        if(firstTime)
        {
            StartCoroutine(firstOneEvent());
            firstTime = false;
        }
        if (hasAlphabet)
        {
            GameManager.Instance.CheckFever(alphabet);
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(alphabet).gameObject.SetActive(false);

        }
    }

    IEnumerator firstOneEvent()
    {
        yield return new WaitForSeconds(1f);
        firstEffect.SetActive(false);
        HoleRenderer.material = afterMet;
        GameManager.Instance.GetFirstOne(index);
    }

    /// <summary>
    /// 피버 글자를 지정해주는 함수
    /// </summary>
    public void SetFeverAlphabet()
    {
        for (int i = 0; i < GameManager.Instance.inGameAlphabet.Length; i++)
        {
            if (!GameManager.Instance.inGameAlphabet[i])
            {
                alphabet = i;
                hasAlphabet = true;
                GameManager.Instance.inGameAlphabet[i] = true;
                transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(true);
                break;
            }
        }
    }

}
