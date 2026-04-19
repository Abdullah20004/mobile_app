using UnityEngine;
using UnityEngine.SceneManagement;
public class pause_menu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
