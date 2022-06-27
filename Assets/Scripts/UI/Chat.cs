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

    [Header("튜토리얼 관련")]
    public Image tutoImg;
    public Sprite[] tutoSprites;

    private void OnEnable()
    {
        GameManager.Instance.isUI = true;
        GameManager.Instance.ShowMole = false;
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
                nextEnable = false;
                break;
            case 3:
                tutoImg.sprite = tutoSprites[0];
                break;
            case 4:
                tutoImg.sprite = tutoSprites[1];
                break;
            case 5:
                tutoImg.sprite = tutoSprites[2];
                break;
            case 6:
                tutoImg.gameObject.SetActive(false);
                EndTutoScene();
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
    /// 튜토리얼 끝 씬
    /// </summary>
    void EndTutoScene()
    {
        talkerTxt.text = "잭";
        chatTxt.text = "이제 됐어?";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
        indexer = 7;
        nextEnable = true;
    }

    /// <summary>
    /// 버튼 클릭 시 튜토리얼 씬
    /// </summary>
    public void Tuto()
    {
        tutoImg.gameObject.SetActive(true);
        nextEnable = true;
        indexer = 3;
    }

    /// <summary>
    /// 버튼 클릭 시 강화 씬
    /// </summary>
    public void Forge()
    {
        if(GameManager.Instance.currentHouseLvl == 0)
        {
            Fail();
        }
        else
        {
            Patch();
        }
    }

    /// <summary>
    /// 버튼 클릭 시 상점 씬
    /// </summary>
    public void Store()
    {
        GameManager.Instance.SetUI(false);

        GameManager.Instance.enterStore = true;

        talkerTxt.text = "잭";
        chatTxt.text = "좋은 집이네..";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
        indexer = 7;
        nextEnable = true;

        GameManager.Instance.ChangeCam(false);
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

    /// <summary>
    /// 집 강화 예외 처리
    /// </summary>
    public void Fail()
    {
        talkerTxt.text = "잭";
        chatTxt.text = "집을 구매 해야 인테리어를 할 수 있어.";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
        indexer = 7;
        nextEnable = true;
    }

    /// <summary>
    /// 패치 중 입니다..
    /// </summary>
    public void Patch()
    {
        talkerTxt.text = "잭";
        chatTxt.text = "가구 배송 중이야 다음 패치까지 기다려줘.";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
        indexer = 7;
        nextEnable = true;
    }

    /// <summary>
    /// UI 종료 함수
    /// </summary>
    public void Exit()
    {
        indexer = 0;
        nextEnable = false;
        GameManager.Instance.isUI = false;
        GameManager.Instance.ShowMole = true;
        gameObject.SetActive(false);
    }
}
