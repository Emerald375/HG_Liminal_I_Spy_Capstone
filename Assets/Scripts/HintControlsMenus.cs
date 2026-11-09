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

    public Transform objectsToFind;
    public Material glowMaterial;
    private Material originalMaterial;
    private MeshRenderer currentObjectMeshRenderer;

    private float glowTimer;
    private bool glowTimerRunning = false;

    public AudioClip hintSound;
    public AudioClip hintCooldownSound;
    public AudioSource audioSource;
    [Range(0f, 10f)]
    [Tooltip("Volume of the selection sound.")]
    public float soundVolume = 10f;

    public Voiceover voiceover;
    private bool voiceoverPlayed = false;

    public bool enablePCControls = true;
    public bool primaryIndexTrigger = true;
    public bool secondaryIndexTrigger = true;

    void Start()
    {
        hintScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);

        hintTimer = 45f;
        timerRunning = true;

        itemsFoundHint = winScreen.itemsFound;
    }

    void Update()
    {
        if (menuController.gameStarted == false) return;

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

                    if (voiceoverPlayed == false)
                    {
                        StartCoroutine(voiceover.PlayHintVoiceover());
                        voiceoverPlayed = true;
                    }
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

        if (glowTimerRunning == true)
        {
            if (glowTimer > 0)
            {
                glowTimer -= Time.deltaTime;
            }
            else
            {
                currentObjectMeshRenderer.material = originalMaterial; // change back to original material
                glowTimerRunning = false;
            }
        }
    }
    public void HintRequested()
    {
        hintScreen.gameObject.SetActive(false);
        PickObjectToGlow();
        PlayHintSound();

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

    public void PickObjectToGlow()
    {
        if (objectsToFind.childCount == 0) return;

        int randomIndex = Random.Range(0, objectsToFind.childCount);
        GameObject randomItem = objectsToFind.GetChild(randomIndex).gameObject;

        Debug.Log("Picked: " + randomItem.name);

        currentObjectMeshRenderer = randomItem.GetComponent<MeshRenderer>();
        if (currentObjectMeshRenderer == null)
        {
            currentObjectMeshRenderer = randomItem.GetComponentInChildren<MeshRenderer>();
        }
        originalMaterial = currentObjectMeshRenderer.material; //save original material
        currentObjectMeshRenderer.material = glowMaterial; // change to glow material

        glowTimerRunning = true;
        glowTimer = 15f;
    }

    private void PlayHintSound()
    {
        if (hintSound == null)
            return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(
                hintSound,
                soundVolume
            );
        }
        else
        {
            AudioSource.PlayClipAtPoint(
                hintSound,
                transform.position,
                soundVolume
            );
        }
    }

    private void PlayHintCooldownSound()
    {
        if (hintSound == null)
            return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(
                hintCooldownSound,
                soundVolume
            );
        }
        else
        {
            AudioSource.PlayClipAtPoint(
                hintCooldownSound,
                transform.position,
                soundVolume
            );
        }
    }

    private void CheckQuestInput()
    {
        /*bool primaryPressed = false;
        bool secondaryPressed = false;
#if UNITY_ANDROID || UNITY_EDITOR

        /*if (primaryIndexTrigger)
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
            HintRequested();
        }*/
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (glowTimerRunning == true)
            {
                PlayHintCooldownSound();
            }
            else
            {
                HintRequested();
            }
        }

        if (OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            ToggleControlsScreen();
        }
    }

    private void CheckPCInput()
    {
        if (!enablePCControls)
            return;


        if (Input.GetMouseButtonDown(2)) //Middle Mouse
        {
            if (glowTimerRunning == true)
            {
                PlayHintCooldownSound();
            }
            else
            {
                HintRequested();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleControlsScreen();
        }
    }

    public void ToggleControlsScreen()
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
}
