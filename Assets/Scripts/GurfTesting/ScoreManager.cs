using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private Text scoreText;
    private int score = 0;

    [Header("Multiplier")]
    [SerializeField] private float multiplierMaxTime = 5f;
    [SerializeField] private Text multiplierText;
    [SerializeField] private Image multiplierBar;
    private float multiplierBarWidth;
    private float multiplierTime = 0f;
    private int multiplier = 1;
    private int multiplierMax = 3;

    public void AddScore(int addedScore)
    {
        score += addedScore * multiplier;
        scoreText.text = "Score: " + score.ToString("D4");
    }


    public void IncreaseMultiplier()
    {
        if(multiplier < multiplierMax)
        {
            multiplier += 1;
        }
        multiplierText.text = multiplier.ToString() + "x";
        multiplierTime = multiplierMaxTime;
        multiplierBar.gameObject.SetActive(true);
    }

    private void DecreaseMultiplier()
    {
       if(multiplierTime <= 0)
       {
            multiplier -= 1;
            multiplierText.text = multiplier.ToString() + "x";

            if(multiplier > 1)
            {
                multiplierTime = multiplierMaxTime;                    
            }
            else
            {
                multiplierBar.gameObject.SetActive(false);
            }
       } 
    }

    private void Start()
    {
        multiplierBarWidth = multiplierBar.GetComponent<RectTransform>().rect.width;
        multiplierBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(multiplierTime > 0)
        {
            multiplierTime -= Time.deltaTime;
            DecreaseMultiplier();

            float multiplierPercentage = multiplierTime / multiplierMaxTime;
            float newBarWidth = multiplierBarWidth * multiplierPercentage;

            Vector2 barSize = multiplierBar.GetComponent<RectTransform>().sizeDelta;
            barSize.x = newBarWidth;
            multiplierBar.GetComponent<RectTransform>().sizeDelta = barSize;
        }
        
        

    }

}
