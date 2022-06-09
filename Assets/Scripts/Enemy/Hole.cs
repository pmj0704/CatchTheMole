using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    float randomTime;
    float spawnTime = 0;

    private void Start()
    {
        SetGravity();
    }

    private void Update()
    {
        if (spawnTime > 0)
        {
            spawnTime -= Time.deltaTime;
        }
        else
        {
            spawnTime = 0;
        }
    }

    public void SummonEnemy()
    {
        StartCoroutine(RandomTimeSpawn());
    }

    private IEnumerator RandomTimeSpawn()
    {
        randomTime = Random.Range(2.0f, 8.0f);
        spawnTime = randomTime;
        yield return new WaitForSeconds(randomTime);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void SetGravity()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Bottom"))
            {
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
