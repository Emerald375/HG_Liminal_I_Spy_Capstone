using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{

    public float timeRemaining = 60f;
    public Text timerText;

    private bool isTimerRunning = false;

    public Text countdownText; 


    void Start()
    {
        timerText.gameObject.SetActive(false);
        StartCountdown();
    }




    void Update()
    {
        if(isTimerRunning)
        {
            timeRemaining = timeRemaining- Time.deltaTime;

            /*if(timeRemaining <= 0)
            {
                Application.Quit()
            }*/
        }
        
        timerText.text = timeRemaining.ToString();
        
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
