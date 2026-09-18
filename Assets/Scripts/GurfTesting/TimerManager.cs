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


    void Start()
    {
        timerText.gameObject.SetActive(false);
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
        StartTimer();

    }

    public void StartCountdown()
    {
        StartCoroutine(Countdown());
    }
}
