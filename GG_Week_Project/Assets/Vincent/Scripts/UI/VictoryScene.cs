using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScene : MonoBehaviour
{
    public string mainMenu;
    public string retryScene;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void LoadRetryScene()
    {
        SceneManager.LoadScene(retryScene);
    }


}
