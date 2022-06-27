using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, InteractiveObj
{

    bool on = false;
    public enum Rotate { X, Y, Z }

    public Rotate doorRotation = Rotate.Z;

    public int angle = 90;

    public bool tp = false;
    public Transform tpPos;
    public Animator Player;

    private void Start()
    {
        DOTween.Init(false, false, LogBehaviour.Default).SetCapacity(100, 20);
    }

    public void Interact(bool interact)
    {
        if (tp)
        {
            Debug.Log("");
            if(!on)
            {
                on = true;
                Player.enabled = false;
                Player.transform.position = tpPos.position;
                StartCoroutine(aniOn());
            }
        }
        else
        {
            Vector3 open = Vector3.zero;
            Vector3 close = Vector3.zero;
            switch (doorRotation)
            {
                case Rotate.X:
                    open = new Vector3(angle, 0f, 0f);
                    close = new Vector3(-angle, 0f, 0f);
                    break;
                case Rotate.Y:
                    open = new Vector3(0f, angle, 0f);
                    close = new Vector3(0f, -angle, 0f);
                    break;
                case Rotate.Z:
                    open = new Vector3(0f, 0f, angle);
                    close = new Vector3(0f, 0f, -angle);
                    break;
                default:
                    break;
            }
            if (on)
            {
                transform.DORotate(open, 1.0f, RotateMode.LocalAxisAdd);
                on = false;
            }
            else
            {
                transform.DORotate(close, 1.0f, RotateMode.LocalAxisAdd);
                on = true;
            }
        }
    }

    IEnumerator aniOn()
    {
        yield return new WaitForSeconds(1f);
        Player.enabled = true;
        on = false;
    }
}
