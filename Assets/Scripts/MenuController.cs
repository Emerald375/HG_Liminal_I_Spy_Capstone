using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private TimerManager timerManager;

    public bool gameStarted = false;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
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
        gameStarted = true;
    }

}
