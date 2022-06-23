using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(!(collision.gameObject.CompareTag("Hammer") || collision.gameObject.CompareTag("Bottom") || collision.gameObject.CompareTag("Player")))
        SendMessageUpwards("HammerCol", true);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!(collision.gameObject.CompareTag("Hammer") || collision.gameObject.CompareTag("Bottom") || collision.gameObject.CompareTag("Player")))
            SendMessageUpwards("HammerCol", false);
    }
}
