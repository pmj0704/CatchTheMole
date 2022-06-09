using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingleTon_01<GameManager>
{
    public Text ScoreTxt;
    private int Score = 0;

    [Header("ภ๛ ฐüทร")]
    public GameObject enemyMole;
    public Transform enemyHolder;
    public int enemyCount = 1;

    void Start()
    {
        UpdateUI();
        SummonMoles();
    }

    void Update()
    {
        
    }

    void UpdateUI()
    {
        ScoreTxt.text = "Score: " + Score.ToString();
    }

    public void AddScore(int addScore)
    {
        this.Score += addScore;
        UpdateUI();
    }

    private void SummonMoles()
    {
        float enemyX = 0;
        float enemyZ = 0;

        for (int i = 0; i < enemyCount; i++)
        {
            enemyX = Random.Range(-70.0f, 70.0f);
            enemyZ = Random.Range(-70.0f, 70.0f);
            Vector3 newPos = new Vector3(enemyX, 1000f, enemyZ);
            Instantiate(enemyMole, enemyHolder).transform.position = newPos;
        }
    }
}
