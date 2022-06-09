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

    //private void OnGUI()
    //{
    //    var labelStyle = new GUIStyle();
    //    labelStyle.fontSize = 15;
    //    labelStyle.normal.textColor = Color.white;
    //    GUI.Label(new Rect(250, 10, 100, 20), "두더지 스폰 대기 시간: " + spawnTime.ToString(),labelStyle);
    //}
}
