using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, InteractiveObj
{

    bool on = false;

    private void Start()
    {
        DOTween.Init(false, false, LogBehaviour.Default).SetCapacity(100, 20);
    }

    public void Interact(bool interact)
    {
        if(on)
        {
            Vector3 open = new Vector3(0f, 0f, 90);
            transform.DORotate(open, 1.0f, RotateMode.LocalAxisAdd);
            on = false;
        }
        else
        {
            Vector3 close = new Vector3(0f, 0f, -90);
            transform.DORotate(close, 1.0f, RotateMode.LocalAxisAdd);
            on = true;
        }
    }
}
