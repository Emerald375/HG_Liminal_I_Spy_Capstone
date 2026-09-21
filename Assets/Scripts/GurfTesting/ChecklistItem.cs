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

    [Range(0f, 1f)]
    [Tooltip("Volume of the selection sound.")]
    public float soundVolume = 1f;


    public void MarkAsFound()
    {
        if(isFound == true)
        {
            return;
        }

        isFound = true;
        checklistText.text = "[X] " + propName;
        
        PlayScribbleSound();
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
