using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintControlsMenus : MonoBehaviour
{
    public GameObject controlsScreen;
    public GameObject hintScreen;

    void Start()
    {
        controlsScreen.gameObject.SetActive(false);
        hintScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void ActivateControlsScreen()
    {
        controlsScreen.gameObject.SetActive(true);
    }

    public void ActivateHintScreen()
    {
        hintScreen.gameObject.SetActive(true);
    }
}
