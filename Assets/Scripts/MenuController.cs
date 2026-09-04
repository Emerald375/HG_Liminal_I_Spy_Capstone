using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject MainMenu;
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ExitGame()
    {
        Debug.Log("game exiting");
        Application.Quit();
    }

    public void StartGame()
    {
        MainMenu.SetActive(false);
    }
}
