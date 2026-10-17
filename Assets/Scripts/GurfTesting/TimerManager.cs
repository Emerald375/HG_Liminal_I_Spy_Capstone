using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{

    private float currentTime = 1f;
    public Text timerText;

    private bool isTimerRunning = false;

    public Text countdownText;

    public GameObject checklist;
    public GameObject teleportSpots;

    public GameObject countdownCanvas;


    void Start()
    {
        checklist.gameObject.SetActive(false);
        teleportSpots.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        countdownCanvas.SetActive(false);
        //StartCountdown();
    }

    void Update()
    {
        if(isTimerRunning)
        {
            currentTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        timerText.gameObject.SetActive(true);
    }

    IEnumerator Countdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1);

        countdownText.text = "2";
        yield return new WaitForSeconds(1);

        countdownText.text = "1";
        yield return new WaitForSeconds(1);

        countdownText.text = "GO";
        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false);
        EnableGameplay();
        StartTimer();
    }

    public void StartCountdown()
    {
        countdownCanvas.SetActive(true);
        countdownText.gameObject.SetActive(true);
        StartCoroutine(Countdown());
    }

    public void EnableGameplay()
    {
        checklist.gameObject.SetActive(true);
        teleportSpots.gameObject.SetActive(true);
    }
}
