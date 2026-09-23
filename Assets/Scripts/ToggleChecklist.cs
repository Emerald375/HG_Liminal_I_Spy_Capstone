using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChecklist : MonoBehaviour
{
    public bool enablePCControls = true;
    public bool primaryIndexTrigger = true;
    public bool secondaryIndexTrigger = true;

    public GameObject checklist;
    
    public void Update()
    {
        CheckPCInput();
        CheckQuestInput();
    }

    private void CheckQuestInput()
    {
        bool primaryPressed = false;
        bool secondaryPressed = false;

#if UNITY_ANDROID || UNITY_EDITOR

        /*if (primaryIndexTrigger)
        {
            primaryPressed = OVRInput.GetDown(
                OVRInput.Button.PrimaryIndexTrigger
            );
        }*/

        if (secondaryIndexTrigger)
        {
            secondaryPressed = OVRInput.GetDown(
                OVRInput.Button.SecondaryIndexTrigger
            );
        }
#endif
        if (primaryPressed || secondaryPressed)
        {
            Toggle();
        }
    }

    private void CheckPCInput()
    {
        if (!enablePCControls)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        bool currentState = checklist.activeSelf;
        checklist.SetActive(!currentState);
        //Debug.Log("Checklist Toggled");
    }
}
