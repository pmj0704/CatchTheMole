using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    float randomTime;

    private void Start()
    {
        SetGravity();
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

    //private void OnGUI()
    //{
    //    var labelStyle = new GUIStyle();
    //    labelStyle.fontSize = 15;
    //    labelStyle.normal.textColor = Color.white;
    //    GUI.Label(new Rect(250, 10, 100, 20), "�δ��� ���� ��� �ð�: " + spawnTime.ToString(),labelStyle);
    //}
}
