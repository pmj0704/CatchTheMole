using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public Text talkerTxt;
    public Text chatTxt;

    public Image jackImg;
    public Image playerImg;

    public GameObject selection;

    private int indexer = 0;
    private bool nextEnable = false;

    public Color Shadowed;
    public Animator iconAniamtion;

    private void OnEnable()
    {
        GameManager.Instance.isUI = true;
        StartCoroutine(whenStart());
        CheckNextScene();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (nextEnable)
            {
                indexer++;
                iconAniamtion.enabled = false;
                nextEnable = false;
                CheckNextScene();
            }
        }
    }

    /// <summary>
    /// 어떤 씬을 시작 할지 판단하는 함수
    /// </summary>
    void CheckNextScene()
    {
        iconAniamtion.enabled = true;
        nextEnable = true;
        switch (indexer)
        {
            case 0:
                FirstScene();
                break;
            case 1:
                SecondScene();
                nextEnable = false;
                break;
            case 2:
                break;
            case 8:
                Exit();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 시작 2초후부터 시나리오를 시작 시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator whenStart()
    {
        yield return new WaitForSeconds(2f);
        nextEnable = true;
    }

    /// <summary>
    /// 첫번째 씬
    /// </summary>
    void FirstScene()
    {
        talkerTxt.text = "잭";
        chatTxt.text = "무슨 일이야?";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
    }

    /// <summary>
    /// 두번째 씬
    /// </summary>
    void SecondScene()
    {
        talkerTxt.text = "나";
        chatTxt.text = "";
        chatTxt.gameObject.SetActive(false);
        jackImg.color = Shadowed;
        playerImg.color = Color.white;
        selection.SetActive(true);
    }

    /// <summary>
    /// 버튼 클릭 시 튜토리얼 씬
    /// </summary>
    public void Tuto()
    {

    }

    /// <summary>
    /// 버튼 클릭 시 강화 씬
    /// </summary>
    public void Forge()
    {

    }

    /// <summary>
    /// 버튼 클릭 시 상점 씬
    /// </summary>
    public void Store()
    {

    }

    /// <summary>
    /// 버튼 클릭 시 뒤로가기
    /// </summary>
    public void Back()
    {
        talkerTxt.text = "잭";
        chatTxt.text = "...";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
        indexer = 7;
        nextEnable = true;
    }

    public void Exit()
    {
        indexer = 0;
        nextEnable = false;
        GameManager.Instance.isUI = false;
        gameObject.SetActive(false);
    }
}
