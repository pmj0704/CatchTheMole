using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Words : MonoBehaviour
{
    float rotateY = 0f;
    public float spd = 100f;

    void Update()
    {
        rotateY += Time.deltaTime * spd;
        Vector3 rotateVec = new Vector3(transform.rotation.eulerAngles.x, rotateY, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(rotateVec);
    }
}
