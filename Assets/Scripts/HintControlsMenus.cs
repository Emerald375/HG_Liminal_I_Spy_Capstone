using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintControlsMenus : MonoBehaviour
{
    public GameObject controlsScreen;
    public GameObject hintScreen;

    public WinScreen winScreen;
    private float itemsFoundHint; //Storing 'items found' value for this script

    public MenuController menuController;

    private float hintTimer; //Timer to toggle ON UI 
    private bool timerRunning = false;

    private float hintTimer2; //Timer to toggle OFF UI 
    private bool timer2Running = false;

    public bool enablePCControls = true;
    public bool primaryIndexTrigger = true;
    public bool secondaryIndexTrigger = true;

    void Start()
    {
        controlsScreen.gameObject.SetActive(false);
        hintScreen.gameObject.SetActive(false);

        hintTimer = 45f;
        timerRunning = true;

        itemsFoundHint = winScreen.itemsFound;
    }

    void Update()
    {
        CheckPCInput();
        CheckQuestInput();

        if (menuController.gameStarted == true && winScreen.gameEnded == false)
        {
            //Timer for hint UI to show up after x amount of time
            if (timerRunning == true)
            {
                if (hintTimer > 0f)
                {
                    hintTimer -= Time.deltaTime;
                    //Debug.Log("timer = " + hintTimer);
                }
                else
                {
                    hintScreen.gameObject.SetActive(true);
                    timerRunning = false;
                    timer2Running = true;
                    hintTimer2 = 30f;
                }
                //reset hintTimer when an item is found
                if (winScreen.itemsFound != itemsFoundHint)
                {
                    hintTimer = 90f;
                    itemsFoundHint = winScreen.itemsFound;
                    //Debug.Log("itemsFound = " + winScreen.itemsFound);
                }
            }
            //Timer for hint UI to go after x amount of time
            else if (timer2Running == true)
            {
                if (hintTimer2 > 0f)
                {
                    hintTimer2 -= Time.deltaTime;
                    //Debug.Log("timer2 = " + hintTimer2);
                }
                else
                {
                    hintScreen.gameObject.SetActive(false);
                    timer2Running = false;
                    timerRunning = true;
                    if (winScreen.itemsFound > 0)
                    {
                        hintTimer = 45f;
                    }
                    else
                    {
                        hintTimer = 90f;
                    }
                }
                //reset hintTimer when an item is found + disables hint UI & hintTimer2
                if (winScreen.itemsFound != itemsFoundHint)
                {
                    hintScreen.gameObject.SetActive(false);
                    timer2Running = false;
                    timerRunning = true;
                    hintTimer = 90f;
                    itemsFoundHint = winScreen.itemsFound;
                    //Debug.Log("itemsFound = " + winScreen.itemsFound);
                }
            }
        }
    }
    public void hintRequested()
    {
        hintScreen.gameObject.SetActive(false);
        //Make random item glow


        timer2Running = false;
        timerRunning = true;
        if (winScreen.itemsFound > 0)
        {
            hintTimer = 20f;
        }
        else
        {
            hintTimer = 10f;
        }
    }

    private void CheckQuestInput()
    {
        bool primaryPressed = false;
        bool secondaryPressed = false;

#if UNITY_ANDROID || UNITY_EDITOR

        if (primaryIndexTrigger)
        {
            primaryPressed = OVRInput.GetDown(
                OVRInput.Button.PrimaryIndexTrigger
            );
        }

        if (secondaryIndexTrigger)
        {
            secondaryPressed = OVRInput.GetDown(
                OVRInput.Button.SecondaryIndexTrigger
            );
        }
#endif
        if (primaryPressed || secondaryPressed)
        {
            hintRequested();
        }
    }

    private void CheckPCInput()
    {
        if (!enablePCControls)
            return;

        if (Input.GetMouseButtonDown(2)) //Middle Mouse
        {
            hintRequested();
        }
    }

    /*public void ToggleControlsScreen()
    {
        if (controlsScreen.activeSelf)
        {
            controlsScreen.gameObject.SetActive(false);
        }
        else
        {
            controlsScreen.gameObject.SetActive(true);
        }
    }

    public void ToggleHintScreen()
    {
        if (controlsScreen.activeSelf)
        {
            hintScreen.gameObject.SetActive(false);
        }
        else
        {
            hintScreen.gameObject.SetActive(true);
        }
    }*/
}
