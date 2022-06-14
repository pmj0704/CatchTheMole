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
    /// �ܺο��� RandomTimeSpawn() �ڷ�ƾ�� ���� �ϱ� ���� �Լ�
    /// </summary>
    public void SummonEnemy()
    {
        StartCoroutine(RandomTimeSpawn());
    }

    /// <summary>
    /// ���� �ð��� �������� �ְ� ��ٸ��� �δ����� ���� ��Ű�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomTimeSpawn()
    {
        randomTime = Random.Range(2.0f, 8.0f);
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

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //�ٴڿ� ������
            if (hit.collider.gameObject.CompareTag("Bottom"))
            {
            //�װ����� �̵�
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
