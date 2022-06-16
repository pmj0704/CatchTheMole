using System.Collections;
using System.Linq;
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
    float nearest = Mathf.Infinity;
    public Transform playerPos;

    float enemyX = 0;
    float enemyZ = 0;

    Vector3 nearestVec = Vector3.zero;

    [Header("UI 관련")]
    public GameObject chatUI;

    //[HideInInspector]
    public bool isUI = false;

    [Header("집 구매 관련")]
    [HideInInspector]
    public int currentHouseLvl = 0;
    public Camera SubCam;
    public Camera MainCam;
    public Text ScorePriceTxt;

    //OnHUI 표시 함수
    public bool ShowMole = true;

    void Start()
    {
        UpdateUI();
        SummonMoles();
    }

    void Update()
    {
        FindNearMole();
    }

    /// <summary>
    /// UI를 업데이트 해주는 함수
    /// </summary>
    void UpdateUI()
    {
        //점수 텍스트 설정
        ScoreTxt.text = "Score: " + Score.ToString();
        ScorePriceTxt.text = "Score: " + Score.ToString();
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
    public int GetScore()
    {
        return Score;
    }

    /// <summary>
    /// 두더지 생성 함수
    /// </summary>
    private void SummonMoles()
    {

        enemyCount = Random.Range(15, 25);

        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(enemyMole, enemyHolder).transform.position = randomTransformSpawn();
        }
    }

    /// <summary>
    /// 두더지를 랜덤한 위치로 이동 시키는 함수
    /// </summary>
    /// <returns></returns>
    public Vector3 randomTransformSpawn()
    {
        enemyX = Random.Range(-70.0f, 70.0f);
        enemyZ = Random.Range(-70.0f, 70.0f);
        //랜덤 X, Z값에 카메라가 안보이는 Y값에 두더지 생성
        Vector3 newPos = new Vector3(enemyX, 1000f, enemyZ);
        return newPos;
    }

    /// <summary>
    /// 상점 UI를 활성화하는 함수
    /// </summary>
    public void OpenStore()
    {
        isUI = true;
        chatUI.SetActive(true);
    }


    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 30;
        labelStyle.normal.textColor = Color.white;

        if (ShowMole)
        {
            GUI.Label(new Rect(250, 10, 100, 20), "필드에 활성화 된 두더지 수: " + FindObjectsOfType<EnemyMove>().Length, labelStyle);
            GUI.Label(new Rect(250, 40, 100, 20), "가장 가까운 두더지와 거리: " + nearest, labelStyle);
            GUI.Label(new Rect(250, 80, 100, 20), "가장 가까운 두더지 좌표: " + nearestVec, labelStyle);
        }
        else
        {
            if(GUI.Button(new Rect(250, 10, 200, 200), "스코어 100 추가"))
            {
                Score += 100;
                UpdateUI();
            }
        }
    }

    /// <summary>
    /// 가장 가까운 두더지를 찾는 함수
    /// </summary>
    private void FindNearMole()
    {

        // 탐색할 오브젝트 목록을 List
        var objArr = GameObject.FindObjectsOfType<Hole>();

        List<Hole> objects = objArr.ToList();

        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = objects
                .OrderBy(obj =>
                {
                    return Vector3.Distance(playerPos.position, obj.transform.position);
                })
            .FirstOrDefault();

        nearest = Vector3.Distance(playerPos.position, neareastObject.transform.position);
        nearestVec = neareastObject.transform.position;
    }

    public void ChangeCam(bool isMain)
    {
        if(isMain)
        {
            SubCam.enabled = false;
            MainCam.enabled = true;
        }
        else
        {
            SubCam.enabled = true;
            MainCam.enabled = false;
        }
    }
}
