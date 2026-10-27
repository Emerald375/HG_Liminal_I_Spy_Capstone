using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public GameObject winScreen;

    public float itemsFound = 0f;
    public float totalItems;

    public bool gameEnded = false;

    public Voiceover voiceover;
    private bool winVoiceover = false;

    [Header("Win Screen Stats")] 
    public ScoreManager scoreManager;
    public TimerManager timerManager;
    public Text scoreText;
    public Text timeText;

    void Start()
    {
        winScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        if (itemsFound == totalItems)
        {
            ActivateWinScreen();
        }
        //Debug.Log(itemsFound);
    }

    public void ItemsFoundCounter()
    {
        itemsFound = itemsFound + 1;
        //Debug.Log("+ 1 found");
    }

    public void ActivateWinScreen()
    {
        timerManager.isTimerRunning = false;
        scoreText.text = scoreManager.score.ToString();
        timeText.text = timerManager.currentTime.ToString();

        int minutes = Mathf.FloorToInt(timerManager.currentTime / 60);
        int seconds = Mathf.FloorToInt(timerManager.currentTime % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        winScreen.gameObject.SetActive(true);
        gameEnded = true;
        if (winVoiceover == false)
        {
            StartCoroutine(voiceover.PlayVictoryVoiceover());
            winVoiceover = true;
        }
    }
}
