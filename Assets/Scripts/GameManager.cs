using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingleTon_01<GameManager>
{
    public Text ScoreTxt;
    private int Score = 0;

    public Transform terrain;

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

    [HideInInspector]
    public bool isUI = false;

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

        enemyCount = Random.Range(15, 25);

        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(enemyMole, enemyHolder).transform.position = randomTransformSpawn();
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
        StartCoroutine(treeCol());
        return newPos;
    }

    /// <summary>
    /// ������ �ݶ��̴��� ���ִ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator treeCol()
    {
        yield return new WaitForSeconds(1f);
        TerrainCollider terrainCollider;

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
        labelStyle.fontSize = 30;
        labelStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(250, 10, 100, 20), "�ʵ忡 Ȱ��ȭ �� �δ��� ��: " + FindObjectsOfType<EnemyMove>().Length, labelStyle);
        GUI.Label(new Rect(250, 40, 100, 20), "���� ����� �δ����� �Ÿ�: " + nearest, labelStyle);
        GUI.Label(new Rect(250, 80, 100, 20), "���� ����� �δ��� ��ǥ: " + nearestVec, labelStyle);
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
}
