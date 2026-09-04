using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistItem : MonoBehaviour
{

    public string propName;
    public Text checklistText;
    private bool isFound = false;


    public void MarkAsFound()
    {
        if(isFound == true)
        {
            return;
        }

        isFound = true;
        checklistText.text = "[X] " + propName;
        
    }



}
