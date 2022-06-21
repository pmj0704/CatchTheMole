using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jack : MonoBehaviour, InteractiveObj
{
    public GameObject text;
    public void Interact(bool interact)
    {
        text.SetActive(interact);
    }
    private void Update()
    {
        if(GameManager.Instance.isUI)
        {
            text.SetActive(false);
        }
    }
}
