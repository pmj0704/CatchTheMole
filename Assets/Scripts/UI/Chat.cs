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

    [Header("Ʃ�丮�� ����")]
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
    /// � ���� ���� ���� �Ǵ��ϴ� �Լ�
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
    /// ���� 2���ĺ��� �ó������� ���� ��Ű�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator whenStart()
    {
        yield return new WaitForSeconds(2f);
        nextEnable = true;
    }

    /// <summary>
    /// ù��° ��
    /// </summary>
    void FirstScene()
    {
        talkerTxt.text = "��";
        chatTxt.text = "���� ���̾�?";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
    }

    /// <summary>
    /// �ι�° ��
    /// </summary>
    void SecondScene()
    {
        talkerTxt.text = "��";
        chatTxt.text = "";
        chatTxt.gameObject.SetActive(false);
        jackImg.color = Shadowed;
        playerImg.color = Color.white;
        selection.SetActive(true);
    }

    /// <summary>
    /// Ʃ�丮�� �� ��
    /// </summary>
    void EndTutoScene()
    {
        talkerTxt.text = "��";
        chatTxt.text = "���� �ƾ�?";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
        indexer = 7;
        nextEnable = true;
    }

    /// <summary>
    /// ��ư Ŭ�� �� Ʃ�丮�� ��
    /// </summary>
    public void Tuto()
    {
        tutoImg.gameObject.SetActive(true);
        nextEnable = true;
        indexer = 3;
    }

    /// <summary>
    /// ��ư Ŭ�� �� ��ȭ ��
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
    /// ��ư Ŭ�� �� ���� ��
    /// </summary>
    public void Store()
    {
        GameManager.Instance.SetUI(false);

        GameManager.Instance.enterStore = true;

        talkerTxt.text = "��";
        chatTxt.text = "���� ���̳�..";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
        indexer = 7;
        nextEnable = true;

        GameManager.Instance.ChangeCam(false);
    }

    /// <summary>
    /// ��ư Ŭ�� �� �ڷΰ���
    /// </summary>
    public void Back()
    {
        talkerTxt.text = "��";
        chatTxt.text = "...";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
        indexer = 7;
        nextEnable = true;
    }

    /// <summary>
    /// �� ��ȭ ���� ó��
    /// </summary>
    public void Fail()
    {
        talkerTxt.text = "��";
        chatTxt.text = "���� ���� �ؾ� ���׸�� �� �� �־�.";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
        indexer = 7;
        nextEnable = true;
    }

    /// <summary>
    /// ��ġ �� �Դϴ�..
    /// </summary>
    public void Patch()
    {
        talkerTxt.text = "��";
        chatTxt.text = "���� ��� ���̾� ���� ��ġ���� ��ٷ���.";
        chatTxt.gameObject.SetActive(true);
        playerImg.color = Shadowed;
        jackImg.color = Color.white;
        selection.SetActive(false);
        indexer = 7;
        nextEnable = true;
    }

    /// <summary>
    /// UI ���� �Լ�
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
