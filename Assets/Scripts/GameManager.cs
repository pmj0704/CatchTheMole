using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingleTon_01<GameManager>
{
    public Text ScoreTxt;
    private int Score = 0;

    [Header("�� ����")]
    public GameObject enemyMole;
    public Transform enemyHolder;
    int enemyCount = 0;


    [Header("UI ����")]
    public GameObject chatUI;

    [HideInInspector]
    public bool isUI = false;

    void Start()
    {
        UpdateUI();
        SummonMoles();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// UI�� ������Ʈ ���ִ� �Լ�
    /// </summary>
    void UpdateUI()
    {
        //���� �ؽ�Ʈ ����
        ScoreTxt.text = "Score: " + Score.ToString();
    }

    /// <summary>
    /// ���� �߰� �Լ�
    /// </summary>
    /// <param name="addScore"></param>
    public void AddScore(int addScore)
    {
        this.Score += addScore;
        UpdateUI();
    }

    /// <summary>
    /// �δ��� ���� �Լ�
    /// </summary>
    private void SummonMoles()
    {
        float enemyX = 0;
        float enemyZ = 0;
        enemyCount = Random.Range(15, 25);

        for (int i = 0; i < enemyCount; i++)
        {
            enemyX = Random.Range(-70.0f, 70.0f);
            enemyZ = Random.Range(-70.0f, 70.0f);
            //���� X, Z���� ī�޶� �Ⱥ��̴� Y���� �δ��� ����
            Vector3 newPos = new Vector3(enemyX, 1000f, enemyZ);
            Instantiate(enemyMole, enemyHolder).transform.position = newPos;
        }
    }

    /// <summary>
    /// ���� UI�� Ȱ��ȭ�ϴ� �Լ�
    /// </summary>
    public void OpenStore()
    {
        Debug.Log("���� ����");
        isUI = true;
        chatUI.SetActive(true);
    }


    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 15;
        labelStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(250, 10, 100, 20), "�ʵ忡 Ȱ��ȭ �� �δ��� ��: " + FindObjectsOfType<EnemyMove>().Length, labelStyle);
    }
}
