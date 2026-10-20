using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voiceover : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    
    [Header("Voiceover Clips")]
    [SerializeField] AudioClip introVoiceover;
    [SerializeField] AudioClip hintVoiceover;
    [SerializeField] AudioClip victoryVoiceover;

    private bool muted = false;

    public bool enablePCControls = true;
    public bool primaryIndexTrigger = true;
    public bool secondaryIndexTrigger = true;

    void Start()
    {
        if (introVoiceover != null || muted == true)
        {
            return;
        }
        else
        {
            audioSource.PlayOneShot(introVoiceover, 1);
        }
    }
    void Update()
    {
        if (muted == false)
        {
            CheckPCInput();
            CheckQuestInput();
        }
    }

    public void PlayHintVoiceover()
    {
        if (hintVoiceover != null || muted == true)
        {
            return;
        }
        else
        {
            audioSource.PlayOneShot(hintVoiceover, 1);
        }
    }
    public void PlayVictoryVoiceover()
    {
        if (victoryVoiceover != null || muted == true)
        {
            return;
        }
        else
        {
            audioSource.PlayOneShot(victoryVoiceover, 1);
        }
    }

    private void CheckQuestInput()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            muted = true;
        }
    }

    private void CheckPCInput()
    {
        if (!enablePCControls)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            muted = true;
        }
    }
}
