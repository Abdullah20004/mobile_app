using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.SceneManagement;
public class main_menu : MonoBehaviour
{
public void PlayGame()
    {
        SceneManager.LoadSceneAsync("game");
    }
public void Quit()
    {
        Application.Quit();
    }
}
