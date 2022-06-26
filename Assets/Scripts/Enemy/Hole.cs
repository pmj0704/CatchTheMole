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
    public float HitBottom;

    public float minSpawnRate = 2.0f;
    public float maxSpawnRate = 8.0f;

    public Transform hitPos;

    public int alphabet = 100;

    public LayerMask layerMask;
    public LayerMask bottomLayerMask;

    private bool hasAlphabet = false;

    private void Start()
    {
        Respawn();
    }

    /// <summary>
    /// �ܺο��� RandomTimeSpawn() �ڷ�ƾ�� ���� �ϱ� ���� �Լ�
    /// </summary>
    public void SummonEnemy()
    {
        hasAlphabet = false;
        StartCoroutine(RandomTimeSpawn());
    }

    /// <summary>
    /// ���� �ð��� �������� �ְ� ��ٸ��� �δ����� ���� ��Ű�� �ڷ�ƾ
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
    /// 1000��ǥ���� �ٴڱ��� �������� �Լ�
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
            //�ٴڿ� ������
            if (hit.collider.gameObject.CompareTag("Bottom"))
            {
                //�װ����� �̵�
                transform.position = new Vector3(transform.position.x, hit.point.y + 0.2f, transform.position.z);
                //transform.rotation = Quaternion.Euler(hit.normal.x, hit.normal.y, transform.rotation.z);
            }
        }
    }

    /// <summary>
    /// ���� �� �δ���
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
                //�δ����� ������ ���� ��ȯ �ǵ��� ������ ã��
                HitLeft = leftHit.point.y;
                HitRight = rightHit.point.y;
                //������ ���� �ƴϸ� �ٽ�
                if (Mathf.Abs(HitLeft - HitRight) > 2)
                {
                    Respawn();
                }
                //�ƴϸ� ������
                else
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, bottomLayerMask))
                    {
                        HitBottom = hit.point.y;
                        if ((HitLeft - HitBottom) > 4 || (HitRight - HitBottom) > 4)
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
    /// �δ����� ����� �ϴ� �Լ�
    /// </summary>
    private void Respawn()
    {
        transform.position = GameManager.Instance.randomTransformSpawn();
        WhenSpawn();
    }

    public void OnCollisionEnter(Collision collision)
    {
        //���� ��ġ�� �ʱ� ���� �Լ�
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Respawn();
        }
    }

    /// <summary>
    /// �ǹ� Ÿ���� ���� �Լ�
    /// </summary>
    public void Fever()
    {
        if (hasAlphabet)
        {
            GameManager.Instance.CheckFever(alphabet);
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(alphabet).gameObject.SetActive(false);

        }
    }


    /// <summary>
    /// �ǹ� ���ڸ� �������ִ� �Լ�
    /// </summary>
    public void SetFeverAlphabet()
    {
        Debug.Log(GameManager.Instance.inGameAlphabet.Length);
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
