using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public string sceneFight;
    private bool Player1HasSelected;
    private bool Player2HasSelected;

    public void SelectPRINCESSPlayer1()
    {
        PlayerManager.instance.classPlayer1 = PlayerManager.CLASS.PRINCESS;
        Player1HasSelected = true;
    }
    public void SelectKNIGHTPlayer1()
    {
        PlayerManager.instance.classPlayer1 = PlayerManager.CLASS.KNIGHT;
        Player1HasSelected = true;
    }
    public void SelectCLOWNPlayer1()
    {
        PlayerManager.instance.classPlayer1 = PlayerManager.CLASS.CLOWN;
        Player1HasSelected = true;
    }
    public void SelectEXECUTIONERPlayer1()
    {
        PlayerManager.instance.classPlayer1 = PlayerManager.CLASS.EXECUTIONER;
        Player1HasSelected = true;
    }
    public void SelectPRINCESSPlayer2()
    {
        PlayerManager.instance.classPlayer2 = PlayerManager.CLASS.PRINCESS;
        Player2HasSelected = true;
    }
    public void SelectKNIGHTPlayer2()
    {
        PlayerManager.instance.classPlayer2 = PlayerManager.CLASS.KNIGHT;
        Player2HasSelected = true;

    }
    public void SelectCLOWNPlayer2()
    {
        PlayerManager.instance.classPlayer2 = PlayerManager.CLASS.CLOWN;
        Player2HasSelected = true;

    }
    public void SelectEXECUTIONERPlayer2()
    {
        PlayerManager.instance.classPlayer2 = PlayerManager.CLASS.EXECUTIONER;
        Player2HasSelected = true;

    }

    public void SaveCharactersAndStartFight()
    {
        if (Player1HasSelected && Player2HasSelected)
        {
            PlayerManager.instance.Save();
            SceneManager.LoadScene(sceneFight);
        }
    }
}
