using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingleTon_01<GameManager>
{
    public Text ScoreTxt;
    private int Score = 0;

    void Start()
    {
        UpdateUI();
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
    }
}
