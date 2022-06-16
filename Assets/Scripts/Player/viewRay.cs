using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewRay : MonoBehaviour
{
    Vector3 currentVec = Vector3.zero;
    bool hitted = true;
    void Update()
    {
        CheckCollision();
    }
    void CheckCollision()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.forward);
        if(Physics.Raycast(ray, out hit, 1.0f))
        {
            if(hit.collider.CompareTag("Bottom") || hit.collider.CompareTag("Wall") && hitted)
            {
                if(hitted)
                {
                    currentVec = transform.position;
                    hitted = false;
                }
            }
            else
            {
                hitted = true;
            }
        }
    }
}
