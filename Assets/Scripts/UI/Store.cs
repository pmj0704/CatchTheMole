using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public GameObject[] gameobjs;
    public int houseTryingToBuy = 1;
    public int[] housePrice = { 500, 1000, 2000, 4000, 8000 };
    public Text buyTxt;
    private bool CanBuy = false;

    public void ShowHouse(int house)
    {
        foreach (var item in gameobjs)
        {
            item.SetActive(false);
        }

        if (house > 0)
        {
            houseTryingToBuy = house - 1;
            gameobjs[houseTryingToBuy].SetActive(true);
        }
    }

    public void Back()
    {
        foreach (var item in gameobjs)
        {
            item.SetActive(false);
        }
        GameManager.Instance.SetUI(true);
        GameManager.Instance.ChangeCam(true);
    }

    private void canBuy()
    {
        if((GameManager.Instance.currentHouseLvl) == houseTryingToBuy )
        {
            if(GameManager.Instance.GetScore() < housePrice[houseTryingToBuy])
            {
                buyTxt.text = "������ �����մϴ�.";
            }
            else
            {
                buyTxt.text = "����";
                CanBuy = true;
            }
        }
        else if((GameManager.Instance.currentHouseLvl) > (houseTryingToBuy))
        {
            buyTxt.text = "���׷��̵� �Ϸ�.";
        }
        else
        {
            buyTxt.text = "�� ���׷��̵尡 �� �ʿ��մϴ�.";
        }
    }

    public void Buy()
    {
        if(CanBuy)
        {
            GameManager.Instance.currentHouseLvl++;
            GameManager.Instance.AddScore(-housePrice[houseTryingToBuy]);
            CanBuy = false;
            GameManager.Instance.SetHouse();
        }
    }

    private void Update()
    {
        canBuy();
        EnteringStore();
    }

    /// <summary>
    /// ������ ��� �� �� �ߵ� �Ǵ� �Լ�
    /// </summary>
    void EnteringStore()
    {
        if (GameManager.Instance.enterStore)
        {
            ShowHouse(GameManager.Instance.currentHouseLvl);
            GameManager.Instance.enterStore = false;
        }
    }
}
