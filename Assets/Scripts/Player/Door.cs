using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, InteractiveObj
{

    bool on = false;

    public void Interact()
    {
        if(on)
        {
            Vector3 open = new Vector3(-90.0f, 0f, 90);
            transform.DORotate(open, 3f);
            on = false;
        }
        else
        {
            Vector3 close = new Vector3(-90.0f, 0f, 180);
            transform.DORotate(close, 3f);
            on = true;
        }
    }
}
