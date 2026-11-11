using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private TimerManager timerManager;

    public bool gameStarted = false;
    private bool startPressed = false;

    private float countdownTimer = 3f;
    private bool countdownRunning = false;

    public AudioSource audioSource;
    public AudioClip buttonAudio;
    public float soundVolume = 10f;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        /*if (startPressed == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.RawButton.B))
            {
                StartGame();
                startPressed = true;
            }
        }*/

        if (countdownRunning != true)
        {
            return;
        }
        else
        {
            if (countdownTimer > 0f)
            {
                countdownTimer -= Time.deltaTime;
            }
            else
            {
                gameStarted = true;
            }
        }
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }*/
    }
    /*public void ExitGame()
    {
        Debug.Log("game exiting");
        Application.Quit();
    }*/

    public void StartGame()
    {
        MainMenu.SetActive(false);
        timerManager.StartCountdown();
        countdownRunning = true;
        audioSource.PlayOneShot(buttonAudio, 1);
    }

    public void PlayUIButtonAudio()
    {
        audioSource.PlayOneShot(buttonAudio, 1);
    }
}
