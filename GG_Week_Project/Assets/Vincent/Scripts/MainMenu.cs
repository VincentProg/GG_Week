using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string SceneToLoad_Play;
    public string SceneToLoad_Credits;

    public void Play()
    {
        if (SceneToLoad_Play != "") 
        {
            SceneManager.LoadScene(SceneToLoad_Play);
            return;
        }
        print("No scene is linked!");
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditsMenu");

    }

    public void Quit()
    {
        Application.Quit();
    }

}
