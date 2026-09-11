using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private Text scoreText;

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;
        scoreText.text = "Score: " + score.ToString("D4");
    }


}
