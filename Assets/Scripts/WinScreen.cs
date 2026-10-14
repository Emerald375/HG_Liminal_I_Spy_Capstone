using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public GameObject winScreen;

    public float itemsFound = 0f;
    public float totalItems;

    public bool gameEnded = false;

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
        winScreen.gameObject.SetActive(true);
        gameEnded = true;
    }
}
