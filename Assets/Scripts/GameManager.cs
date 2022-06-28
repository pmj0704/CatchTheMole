using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public bool inHouse;
    public GameObject mainUI;
    public bool enterStore = false;
    private Store store = null;

    //OnGUI ǥ�� �Լ�
    public bool ShowMole = true;

    [Header("�ǹ� ����")]
    public Text[] feverTxt;
    public Color[] feverColor;
    public bool[] fever = { false, false, false, false, false };
    public bool isFeverTime = false;
    public bool isFever = false;
    public bool[] inGameAlphabet = { false, false, false, false, false };
    private int[] randomMoles = { 100, 100, 100, 100, 100 };

    [Header("�δ��� ã�� ����")]
    public Text foundTxt;
    private int foundedMoles = 0;

    [Header("���� ����")]
    public GameObject Title;
    public bool isEsc = true;
    private AudioSource[] audioSources = null;
    public AudioListener audioListener = null;
    private float currentAudioLength1 = 0f;
    private float currentAudioLength2 = 0f;
    private float currentAudioLength3 = 0f;
    private bool isMute = false;
    public Sprite[] muteSprites;

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        store = FindObjectOfType<Store>();
        UpdateUI();
        SummonMoles();
    }

    void Update()
    {
        FindNearMole();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isEsc)
            {
                gameStart();
            }
            else
            {
                Title.SetActive(true);
                Time.timeScale = 0;
                isEsc = true;
                if(isFever)
                {
                    currentAudioLength3 = audioSources[2].time;
                    audioSources[2].Stop();
                    audioSources[0].Play();
                    audioSources[0].time = currentAudioLength1;
                }
                else
                {
                    currentAudioLength2 = audioSources[1].time;
                    audioSources[1].Stop();
                    audioSources[0].Play();
                    audioSources[0].time = currentAudioLength1;
                }
            }
        }
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
        if (isFever) this.Score += addScore;
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
            summoningMole.GetComponent<Hole>().index = i;
        }
        SummonFever();
    }

    /// <summary>
    /// �ǹ� �δ����� �������ִ� �Լ�
    /// </summary>
    private void SetFeverMole(int i)
    {
        RandomMole();
        enemyHolder.GetChild(randomMoles[i]).GetComponent<Hole>().SetFeverAlphabet();
    }

    /// <summary>
    /// ���� �δ����� �������ִ� �Լ�
    /// </summary>
    /// <returns></returns>
    private void RandomMole()
    {
        int randomMole = Random.Range(0, enemyCount);
        for (int i = 0; i < randomMoles.Length; i++)
        {
            if (randomMole == randomMoles[i])
            {
                randomMole = Random.Range(0, enemyCount);
                continue;
            }
        }
        for (int i = 0; i < randomMoles.Length; i++)
        {
            if (randomMoles[i] == 100)
            {
                randomMoles[i] = randomMole;
                break;
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
            if (GUI.Button(new Rect(250, 120, 200, 40), "�ǹ� Ÿ��"))
            {
                isFeverTime = true;
                isFever = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(250, 10, 200, 200), "���ھ� 100 �߰�"))
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
        if (isMain)
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
            if (i == currentHouseLvl - 1)
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

    /// <summary>
    /// �ǹ� ������Ʈ �Լ�
    /// </summary>
    /// <param name="index"></param>
    public void CheckFever(int index)
    {
        fever[index] = true;
        inGameAlphabet[index] = false;
        feverTxt[index].color = feverColor[index];
        if (fever[0] && fever[1] && fever[2] && fever[3] && fever[4])
        {
            isFeverTime = true;
                isFever = true;
        }
    }

    /// <summary>
    /// �ǹ� UI ���������� �ϱ�
    /// </summary>
    public void BlackFeverUI()
    {
        for (int i = 0; i < feverTxt.Length; i++)
        {
            feverTxt[i].color = Color.black;
        }
    }

    /// <summary>
    /// �ǹ� ���� ����� �Լ�
    /// </summary>
    public void SummonFever()
    {
        for (int i = 0; i < 5; i++)
        {
            fever[i] = false;
            inGameAlphabet[i] = false;
            randomMoles[i] = 100;
            SetFeverMole(i);
        }
    }

    /// <summary>
    /// ó�� ���� �δ����� ���� �Լ�
    /// </summary>
    public void GetFirstOne(int index)
    {
        foundedMoles++;
        if (foundedMoles < enemyCount)
        {
            foundTxt.text = "�δ��� �߰�! (" + foundedMoles + "/" + enemyCount + ")";
        }
        else
        {
            foundTxt.text = "��� �δ��� �߰�!! (" + foundedMoles + "/" + enemyCount + ")     (+ 1000 Point)";
            AddScore(1000);
        }
        foundTxt.DOFade(1f, 1f);
        StartCoroutine(fadeAfter());
    }

    /// <summary>
    /// �ؽ�Ʈ�� ���� �ڿ� ���̵� �Ǳ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator fadeAfter()
    {
        yield return new WaitForSeconds(1f);
        foundTxt.DOFade(0f, 1f);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void Quit()
    {
        Application.Quit();    
    }

    /// <summary>
    /// ���� ������ ����ϴ� �Լ�
    /// </summary>
    public void gameStart()
    {
        
        Title.SetActive(false);
        Time.timeScale = 1;
        isEsc = false;
        if (!isFever)
        {
            currentAudioLength1 = audioSources[0].time;
            audioSources[0].Stop();
            audioSources[1].Play();
            audioSources[1].time = currentAudioLength2;
        }
        else
        {
            currentAudioLength1 = audioSources[0].time;
            audioSources[0].Stop();
            audioSources[2].Play();
            audioSources[2].time = currentAudioLength3;
        }
    }

    /// <summary>
    /// �ǹ��� ���� �Ҷ� �뷡�� ����/��� �ǵ����ϴ� �Լ�
    /// </summary>
    public void atFever()
    {
        currentAudioLength2 = audioSources[1].time;
        audioSources[1].Stop();
        audioSources[2].Play();
    }

    /// <summary>
    /// ���Ͱ� ������ �뷡�� ����/��� �ǵ����ϴ� �Լ�
    /// </summary>
    public void FeverEnd()
    {
        audioSources[2].Stop();
        audioSources[1].Play();
        audioSources[1].time = currentAudioLength2;

    }

    /// <summary>
    /// ���Ұ� ��ư
    /// </summary>
    /// <param name="img"></param>
    public void Mute(Image img)
    {
        if(isMute)
        {
            audioSources[0].mute = false;
            audioSources[1].mute = false;
            audioSources[2].mute = false;
            isMute = false;
            img.sprite = muteSprites[0];
        }
        else
        {
            audioSources[0].mute = true;
            audioSources[1].mute = true;
            audioSources[2].mute = true;
            isMute = true;
            img.sprite = muteSprites[1];
        }
    }

    public void PlayerAtk()
    {
        audioSources[3].Play();
    }

    public void changeSound(Slider slider)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = slider.value;
        }
    }
}