using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public GameObject[] gameobjs;
    public int houseTryingToBuy;
    public int[] housePrice = { 500, 1000, 2000, 4000, 8000 };
    public Text buyTxt;
    public void ShowHouse(int house)
    {
        houseTryingToBuy = house;
        foreach (var item in gameobjs)
        {
            item.SetActive(false);
        }
        gameobjs[house].SetActive(true);
    }

    public void Back()
    {
        foreach (var item in gameobjs)
        {
            item.SetActive(false);
        }
        GameManager.Instance.ChangeCam(true);
    }

    private void canBuy()
    {
        if((GameManager.Instance.currentHouseLvl - 1) == houseTryingToBuy)
        {
            if(GameManager.Instance.GetScore() < housePrice[houseTryingToBuy])
            {
                buyTxt.text = "금액이 부족합니다.";
            }
            buyTxt.text = "구매";
        }
        else
        {
            buyTxt.text = "집 업그레이드가 더 필요합니다.";
        }
    }

    private void Update()
    {
        canBuy();
    }
}
