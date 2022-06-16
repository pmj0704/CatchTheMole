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
    /// ��ư Ŭ�� �� Ʃ�丮�� ��
    /// </summary>
    public void Tuto()
    {

    }

    /// <summary>
    /// ��ư Ŭ�� �� ��ȭ ��
    /// </summary>
    public void Forge()
    {

    }

    /// <summary>
    /// ��ư Ŭ�� �� ���� ��
    /// </summary>
    public void Store()
    {

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

    public void Exit()
    {
        indexer = 0;
        nextEnable = false;
        GameManager.Instance.isUI = false;
        gameObject.SetActive(false);
    }
}
