using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SlectorMenu : MonoBehaviour
{
    public TextMeshProUGUI KnightP1;
    public TextMeshProUGUI PrincessP1;
    public TextMeshProUGUI ExecutionerP1;
    public TextMeshProUGUI JesterP1;

    public TextMeshProUGUI KnightP2;
    public TextMeshProUGUI PrincessP2;
    public TextMeshProUGUI ExecutionerP2;
    public TextMeshProUGUI JesterP2;

    public string sceneFight;
    private bool Player1HasSelected;
    private bool Player2HasSelected;

    public KeyCode keyPlay;
    public GameObject choseCharacter;


    void Start()
    {
        KnightP1.enabled = false;
        KnightP2.enabled = false;
        PrincessP1.enabled = false;
        PrincessP2.enabled = false;
        ExecutionerP1.enabled = false;
        ExecutionerP2.enabled = false;
        JesterP1.enabled = false;
        JesterP2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyPlay))
        { 
           SaveCharactersAndStartFight();
        }
    }

    public void KnightPlayer1()
    {
        KnightP1.enabled = true;
        PrincessP1.enabled = false;
        ExecutionerP1.enabled = false;
        JesterP1.enabled = false;

        PlayerManager.instance.classPlayer1 = PlayerManager.CLASS.KNIGHT;
        Player1HasSelected = true;
    }

    public void PrincessPlayer1()
    {
        KnightP1.enabled = false;
        PrincessP1.enabled = true;
        ExecutionerP1.enabled = false;
        JesterP1.enabled = false;

        PlayerManager.instance.classPlayer1 = PlayerManager.CLASS.PRINCESS;
        Player1HasSelected = true;
    }

    public void ExecutionerPlayer1()
    {
        KnightP1.enabled = false;
        PrincessP1.enabled = false;
        ExecutionerP1.enabled = true;
        JesterP1.enabled = false;

        PlayerManager.instance.classPlayer1 = PlayerManager.CLASS.EXECUTIONER;
        Player1HasSelected = true;
    }

    public void JesterPlayer1()
    {
        KnightP1.enabled = false;
        PrincessP1.enabled = false;
        ExecutionerP1.enabled = false;
        JesterP1.enabled = true;

        PlayerManager.instance.classPlayer1 = PlayerManager.CLASS.CLOWN;
        Player1HasSelected = true;
    }

    public void KnightPlayer2()
    {
        KnightP2.enabled = true;
        PrincessP2.enabled = false;
        ExecutionerP2.enabled = false;
        JesterP2.enabled = false;

        PlayerManager.instance.classPlayer2 = PlayerManager.CLASS.KNIGHT;
        Player2HasSelected = true;
    }

    public void PrincessPlayer2()
    {
        KnightP2.enabled = false;
        PrincessP2.enabled = true;
        ExecutionerP2.enabled = false;
        JesterP2.enabled = false;

        PlayerManager.instance.classPlayer2 = PlayerManager.CLASS.PRINCESS;
        Player2HasSelected = true;
    }

    public void ExecutionerPlayer2()
    {
        KnightP2.enabled = false;
        PrincessP2.enabled = false;
        ExecutionerP2.enabled = true;
        JesterP2.enabled = false;

        PlayerManager.instance.classPlayer2 = PlayerManager.CLASS.EXECUTIONER;
        Player2HasSelected = true;
    }

    public void JesterPlayer2()
    {
        KnightP2.enabled = false;
        PrincessP2.enabled = false;
        ExecutionerP2.enabled = false;
        JesterP2.enabled = true;

        PlayerManager.instance.classPlayer2 = PlayerManager.CLASS.CLOWN;
        Player2HasSelected = true;
    }
    public void SaveCharactersAndStartFight()
    {
        if (Player1HasSelected && Player2HasSelected)
        {
            PlayerManager.instance.Save();
            StartCoroutine(LaunchFight());
        }
        else StartCoroutine("Warning");
    }

    IEnumerator Warning()
    {
        choseCharacter.SetActive(true);
        yield return new WaitForSeconds(2);
        choseCharacter.SetActive(false);
    }

    IEnumerator LaunchFight()
    {
        AudioManager.instance.Play("Pig");
        yield return new WaitForSeconds(1);
        AudioManager.instance.StopPlaying("Theme");
        AudioManager.instance = null;
        SceneManager.LoadScene(sceneFight);
    }
}
