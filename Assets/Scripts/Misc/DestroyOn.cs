using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOn : MonoBehaviour
{
    public float destroyTime = 2f;
    
    void Start()
    {
        StartCoroutine(destroyOn());
    }

    IEnumerator destroyOn()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
