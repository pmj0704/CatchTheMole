using System.Collections;
using System.Linq;
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
    float nearest = Mathf.Infinity;
    public Transform playerPos;

    float enemyX = 0;
    float enemyZ = 0;

    Vector3 nearestVec = Vector3.zero;

    [Header("UI ����")]
    public GameObject chatUI;

    //[HideInInspector]
    public bool isUI = false;

    [Header("�� ���� ����")]
    public int currentHouseLvl = 0;
    public Camera SubCam;
    public Camera MainCam;
    public Text ScorePriceTxt;
    public Transform house;
    public GameObject houseUI;
    public GameObject mainUI;
    public bool enterStore = false;
    private Store store = null;

    //OnGUI ǥ�� �Լ�
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
    /// UI�� ������Ʈ ���ִ� �Լ�
    /// </summary>
    void UpdateUI()
    {
        //���� �ؽ�Ʈ ����
        ScoreTxt.text = "Score: " + Score.ToString();
        ScorePriceTxt.text = "Score: " + Score.ToString();
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
    public int GetScore()
    {
        return Score;
    }

    /// <summary>
    /// �δ��� ���� �Լ�
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
    /// �δ����� ������ ��ġ�� �̵� ��Ű�� �Լ�
    /// </summary>
    /// <returns></returns>
    public Vector3 randomTransformSpawn()
    {
        enemyX = Random.Range(-70.0f, 70.0f);
        enemyZ = Random.Range(-70.0f, 70.0f);
        //���� X, Z���� ī�޶� �Ⱥ��̴� Y���� �δ��� ����
        Vector3 newPos = new Vector3(enemyX, 1000f, enemyZ);
        return newPos;
    }

    /// <summary>
    /// ���� UI�� Ȱ��ȭ�ϴ� �Լ�
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
            GUI.Label(new Rect(250, 10, 100, 20), "�ʵ忡 Ȱ��ȭ �� �δ��� ��: " + FindObjectsOfType<EnemyMove>().Length, labelStyle);
            GUI.Label(new Rect(250, 40, 100, 20), "���� ����� �δ����� �Ÿ�: " + nearest, labelStyle);
            GUI.Label(new Rect(250, 80, 100, 20), "���� ����� �δ��� ��ǥ: " + nearestVec, labelStyle);
        }
        else
        {
            if(GUI.Button(new Rect(250, 10, 200, 200), "���ھ� 100 �߰�"))
            {
                Score += 100;
                UpdateUI();
            }
        }
    }

    /// <summary>
    /// ���� ����� �δ����� ã�� �Լ�
    /// </summary>
    private void FindNearMole()
    {

        // Ž���� ������Ʈ ����� List
        var objArr = GameObject.FindObjectsOfType<Hole>();

        List<Hole> objects = objArr.ToList();

        // LINQ �޼ҵ带 �̿��� ���� ����� ���� ã���ϴ�.
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
    /// �� ���� ���� ���� ���� ī�޶�� �̵��ϴ� �Լ�
    /// </summary>
    /// <param name="isMain">���� �� ON?</param>
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
    /// ������ ���� ���� ���� ǥ���ϴ� �Լ�
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
    /// ���θ� Ű�� �� UI�� ���� �Լ�
    /// </summary>
    /// <param name="main">���� UI ON?</param>
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
