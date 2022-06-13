using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingleTon_01<GameManager>
{
    public Text ScoreTxt;
    private int Score = 0;

    [Header("적 관련")]
    public GameObject enemyMole;
    public Transform enemyHolder;
    int enemyCount = 0;


    [Header("UI 관련")]
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
    /// UI를 업데이트 해주는 함수
    /// </summary>
    void UpdateUI()
    {
        //점수 텍스트 설정
        ScoreTxt.text = "Score: " + Score.ToString();
    }

    /// <summary>
    /// 점수 추가 함수
    /// </summary>
    /// <param name="addScore"></param>
    public void AddScore(int addScore)
    {
        this.Score += addScore;
        UpdateUI();
    }

    /// <summary>
    /// 두더지 생성 함수
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
            //랜덤 X, Z값에 카메라가 안보이는 Y값에 두더지 생성
            Vector3 newPos = new Vector3(enemyX, 1000f, enemyZ);
            Instantiate(enemyMole, enemyHolder).transform.position = newPos;
        }
    }

    /// <summary>
    /// 상점 UI를 활성화하는 함수
    /// </summary>
    public void OpenStore()
    {
        Debug.Log("상점 오픈");
        isUI = true;
        chatUI.SetActive(true);
    }


    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 15;
        labelStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(250, 10, 100, 20), "필드에 활성화 된 두더지 수: " + FindObjectsOfType<EnemyMove>().Length, labelStyle);
    }
}
