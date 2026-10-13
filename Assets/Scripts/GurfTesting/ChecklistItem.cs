using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistItem : MonoBehaviour
{

    public string propName;
    public Text checklistText;
    private bool isFound = false;

    public AudioClip scribbleSound;
    public AudioSource audioSource;

    [Range(0f, 10f)]
    [Tooltip("Volume of the selection sound.")]
    public float soundVolume = 10f;

    public WinScreen winScreen;

    public void MarkAsFound()
    {
        if(isFound == true)
        {
            return;
        }

        isFound = true;
        checklistText.text = "[X] " + propName;
        
        PlayScribbleSound();
        winScreen.ItemsFoundCounter();
    }

    private void PlayScribbleSound()
    {
        if (scribbleSound == null)
            return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(
                scribbleSound,
                soundVolume
            );
        }
        else
        {
            AudioSource.PlayClipAtPoint(
                scribbleSound,
                transform.position,
                soundVolume
            );
        }
    }
}
