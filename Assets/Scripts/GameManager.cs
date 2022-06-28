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
    public bool inHouse;
    public GameObject mainUI;
    public bool enterStore = false;
    private Store store = null;

    //OnGUI 표시 함수
    public bool ShowMole = true;

    [Header("피버 관련")]
    public Text[] feverTxt;
    public Color[] feverColor;
    public bool[] fever = { false, false, false, false, false };
    public bool isFeverTime = false;
    public bool isFever = false;
    public bool[] inGameAlphabet = { false, false, false, false, false };
    private int[] randomMoles = { 100, 100, 100, 100, 100 };

    [Header("두더지 찾기 관련")]
    public Text foundTxt;
    private int foundedMoles = 0;

    [Header("설정 관련")]
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
        if (isFever) this.Score += addScore;
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
            summoningMole.GetComponent<Hole>().index = i;
        }
        SummonFever();
    }

    /// <summary>
    /// 피버 두더지를 지정해주는 함수
    /// </summary>
    private void SetFeverMole(int i)
    {
        RandomMole();
        enemyHolder.GetChild(randomMoles[i]).GetComponent<Hole>().SetFeverAlphabet();
    }

    /// <summary>
    /// 랜덤 두더지를 지정해주는 함수
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
            if (GUI.Button(new Rect(250, 120, 200, 40), "피버 타임"))
            {
                isFeverTime = true;
                isFever = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(250, 10, 200, 200), "스코어 100 추가"))
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
    /// 구매한 집을 메인 씬에 표시하는 함수
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
    /// 메인를 키고 집 UI를 끄는 함수
    /// </summary>
    /// <param name="main">메인 UI ON?</param>
    public void SetUI(bool main)
    {
        mainUI.SetActive(main);
        houseUI.SetActive(!main);
    }

    /// <summary>
    /// 피버 업데이트 함수
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
    /// 피버 UI 검정색으로 하기
    /// </summary>
    public void BlackFeverUI()
    {
        for (int i = 0; i < feverTxt.Length; i++)
        {
            feverTxt[i].color = Color.black;
        }
    }

    /// <summary>
    /// 피버 글자 재생성 함수
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
    /// 처음 잡은 두더지를 위한 함수
    /// </summary>
    public void GetFirstOne(int index)
    {
        foundedMoles++;
        if (foundedMoles < enemyCount)
        {
            foundTxt.text = "두더지 발견! (" + foundedMoles + "/" + enemyCount + ")";
        }
        else
        {
            foundTxt.text = "모든 두더지 발견!! (" + foundedMoles + "/" + enemyCount + ")     (+ 1000 Point)";
            AddScore(1000);
        }
        foundTxt.DOFade(1f, 1f);
        StartCoroutine(fadeAfter());
    }

    /// <summary>
    /// 텍스트가 몇초 뒤에 페이드 되기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator fadeAfter()
    {
        yield return new WaitForSeconds(1f);
        foundTxt.DOFade(0f, 1f);
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void Quit()
    {
        Application.Quit();    
    }

    /// <summary>
    /// 게임 시작을 담당하는 함수
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
    /// 피버가 시작 할때 노래가 중지/재생 되도록하는 함수
    /// </summary>
    public void atFever()
    {
        currentAudioLength2 = audioSources[1].time;
        audioSources[1].Stop();
        audioSources[2].Play();
    }

    /// <summary>
    /// 피터가 끝나고 노래가 중지/재생 되도록하는 함수
    /// </summary>
    public void FeverEnd()
    {
        audioSources[2].Stop();
        audioSources[1].Play();
        audioSources[1].time = currentAudioLength2;

    }

    /// <summary>
    /// 음소거 버튼
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