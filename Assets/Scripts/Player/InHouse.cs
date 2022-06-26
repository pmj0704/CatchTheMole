using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHouse : MonoBehaviour
{
    public Transform camView;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 newPos = new Vector3(0, 1, 0);
            camView.localPosition = newPos;
            GameManager.Instance.inHouse = true;
            Camera.main.GetComponent<CameraCtrl>().distance = 1.5f;
            Camera.main.GetComponent<CameraCtrl>().height = 0.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 newPos = new Vector3(0, 2, 0);
            camView.localPosition = newPos;
            GameManager.Instance.inHouse = false;
        }
    }
}
