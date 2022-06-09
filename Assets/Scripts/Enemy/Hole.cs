using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    float randomTime;
    float spawnTime = 0;

    private void Update()
    {
        if(spawnTime > 0)
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
        randomTime = Random.RandomRange(2.0f, 8.0f);
        spawnTime = randomTime;
        yield return new WaitForSeconds(randomTime);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 15;
        labelStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(250, 10, 100, 20), "두더지 스폰 대기 시간: " + spawnTime.ToString(),labelStyle);
    }
}
