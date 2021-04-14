using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        
    }

    public void KnightPlayer1()
    {
        KnightP1.enabled = true;
        PrincessP1.enabled = false;
        ExecutionerP1.enabled = false;
        JesterP1.enabled = false;
    }

    public void PrincessPlayer1()
    {
        KnightP1.enabled = false;
        PrincessP1.enabled = true;
        ExecutionerP1.enabled = false;
        JesterP1.enabled = false;
    }

    public void ExecutionerPlayer1()
    {
        KnightP1.enabled = false;
        PrincessP1.enabled = false;
        ExecutionerP1.enabled = true;
        JesterP1.enabled = false;
    }

    public void JesterPlayer1()
    {
        KnightP1.enabled = false;
        PrincessP1.enabled = false;
        ExecutionerP1.enabled = false;
        JesterP1.enabled = true;
    }

    public void KnightPlayer2()
    {
        KnightP2.enabled = true;
        PrincessP2.enabled = false;
        ExecutionerP2.enabled = false;
        JesterP2.enabled = false;
    }

    public void PrincessPlayer2()
    {
        KnightP2.enabled = false;
        PrincessP2.enabled = true;
        ExecutionerP2.enabled = false;
        JesterP2.enabled = false;
    }

    public void ExecutionerPlayer2()
    {
        KnightP2.enabled = false;
        PrincessP2.enabled = false;
        ExecutionerP2.enabled = true;
        JesterP2.enabled = false;
    }

    public void JesterPlayer2()
    {
        KnightP2.enabled = false;
        PrincessP2.enabled = false;
        ExecutionerP2.enabled = false;
        JesterP2.enabled = true;
    }
}
