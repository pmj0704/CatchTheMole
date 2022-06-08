using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SummonEnemy()
    {
        StartCoroutine(RandomTimeSpawn());
    }

    private IEnumerator RandomTimeSpawn()
    {
        float randomTime = Random.RandomRange(2.0f, 8.0f);
        yield return new WaitForSeconds(randomTime);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
