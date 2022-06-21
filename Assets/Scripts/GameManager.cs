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
    public int currentHouseLvl = 0;
    public Camera SubCam;
    public Camera MainCam;
    public Text ScorePriceTxt;
    public Transform house;
    public GameObject houseUI;
    public GameObject mainUI;
    public bool enterStore = false;
    private Store store = null;

    //OnGUI 표시 함수
    public bool ShowMole = true;

    public enum Alphabet
    {
        F, E_Y, V, E_B, R, NULL
    };


    void Start()
    {
        store = FindObjectOfType<Store>();
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
            GameObject summoningMole = Instantiate(enemyMole, enemyHolder);
            summoningMole.transform.position = randomTransformSpawn();
            if(i < 5)
            {
                summoningMole.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
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

    /// <summary>
    /// 집 구매 씬과 메인 씬을 카메라로 이동하는 함수
    /// </summary>
    /// <param name="isMain">메인 씬 ON?</param>
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

    /// <summary>
    /// 구매한 집을 메인 씬에 표시하는 함수
    /// </summary>
    public void SetHouse()
    {
        for (int i = 0; i < house.transform.childCount; i++)
        {
            if(i == currentHouseLvl-1)
            {
                house.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                house.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 메인를 키고 집 UI를 끄는 함수
    /// </summary>
    /// <param name="main">메인 UI ON?</param>
    public void SetUI(bool main)
    {
        mainUI.SetActive(main);
        houseUI.SetActive(!main);
    }

    public void GetAlphabet(Alphabet alphabet)
    {
        switch (alphabet)
        {
            case Alphabet.F:
                break;
            case Alphabet.E_Y:
                break;
            case Alphabet.V:
                break;
            case Alphabet.E_B:
                break;
            case Alphabet.R:
                break;
            case Alphabet.NULL:
                break;
            default:
                break;
        }
    }
}
