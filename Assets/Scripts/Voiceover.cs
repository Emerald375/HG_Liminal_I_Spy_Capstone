using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Voiceover : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    
    [Header("Voiceover Clips")]
    [SerializeField] AudioClip introVoiceover;
    [SerializeField] AudioClip instructionsVoiceover;
    [SerializeField] AudioClip controlsVoiceover;
    [SerializeField] AudioClip hintVoiceover;
    [SerializeField] AudioClip victoryVoiceover;

    [Range(0f, 10f)]
    [Tooltip("Volume of the selection sound.")]
    public float soundVolume = 10f;

    private bool muted = false;

    public GameObject textBox;
    public GameObject subtitleBox;

    public bool enablePCControls = true;
    public bool primaryIndexTrigger = true;
    public bool secondaryIndexTrigger = true;

    void Start()
    {
        textBox.GetComponent<Text>().text = "";
        subtitleBox.SetActive(false);
        if (muted == true)
        {
            return;
        }
        else
        {
            StartCoroutine(PlayIntroVoiceover());
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

    IEnumerator PlayIntroVoiceover()
    { 
        yield return new WaitForSeconds(2f);

        if (muted == false)
        {
            audioSource.PlayOneShot(introVoiceover, 1);
            subtitleBox.SetActive(true);
            textBox.GetComponent<Text>().text = "Welcome to YouSpy. Recentre your mind as you search the room for the items listed on your checklist.";
            yield return new WaitForSeconds(6.5f);
        }
        if (muted == false)
        {
            audioSource.PlayOneShot(instructionsVoiceover, 1);
            textBox.GetComponent<Text>().text = "Earn a higher score by finding items in rapid succession. Don’t forget to open and rummage through the draws, you never know what might find in those.";
            yield return new WaitForSeconds(9f);
        }
        if (muted == false)
        {
            audioSource.PlayOneShot(controlsVoiceover, 1);
            textBox.GetComponent<Text>().text = "If you’re not sure what buttons to press, press the ‘Start’ button on your left-hand controller to toggle on the button map menu.";
            yield return new WaitForSeconds(6f);
        }
        textBox.GetComponent<Text>().text = "";
        subtitleBox.SetActive(false);
    }
    
    public IEnumerator PlayHintVoiceover()
    {
        if (muted == false)
        {
            audioSource.PlayOneShot(hintVoiceover, 1);
            subtitleBox.SetActive(true);
            textBox.GetComponent<Text>().text = "Need a hand? Press the ‘A’ button on your right-hand controller for a hint.";
            yield return new WaitForSeconds(4f);
            textBox.GetComponent<Text>().text = "";
            subtitleBox.SetActive(false);
        }
    }
    public IEnumerator PlayVictoryVoiceover()
    {
        if (muted == false)
        {
            audioSource.PlayOneShot(victoryVoiceover, 1);
            subtitleBox.SetActive(true);
            textBox.GetComponent<Text>().text = "Congratulations, you found everything. Try again to see if you can beat your score and time. Although, you won’t find everything in the same places as last time.";
            yield return new WaitForSeconds(8.2f);
            textBox.GetComponent<Text>().text = "";
            subtitleBox.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.M))
        {
            muted = true;
            Debug.Log("muted voiceovers");
        }
    }
}
