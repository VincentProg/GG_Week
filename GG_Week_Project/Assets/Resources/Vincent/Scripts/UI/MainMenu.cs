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
        SceneManager.LoadScene("SelectorMenu");
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
