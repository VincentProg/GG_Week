using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;

    public Player player1;
    public Player player2;

    public Transform transformPlayer1;
    public Transform transformPlayer2;

    //[HideInInspector]
    //public enum WEAPON { PUNCH, SWORD, ARC, PIG }

    public enum CLASS { PRINCESS, KNIGHT, CLOWN, EXECUTIONER}


    public CLASS classPlayer1;
    public CLASS classPlayer2;

    public Animator princessAnim;
    public Animator knightAnim;
    public Animator clownAnim;
    public Animator executionerAnim;


    public Transform respawnP1;
    public Transform respawnP2;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        LoadSave();

        if (player1 != null)
            switch (classPlayer1)
            {
                case CLASS.PRINCESS:
                    player1.anim = princessAnim;
                    break;
                case CLASS.KNIGHT:
                    player1.anim = knightAnim;
                    break;
                case CLASS.CLOWN:
                    player1.anim = clownAnim;
                    break;
                case CLASS.EXECUTIONER:
                    player1.anim = executionerAnim;
                    break;
            }

        if(player2 != null)
        switch (classPlayer2)
        {
            case CLASS.PRINCESS:
                player2.anim = princessAnim;
                break;
            case CLASS.KNIGHT:
                player2.anim = knightAnim;
                break;
            case CLASS.CLOWN:
                player2.anim = clownAnim;
                break;
            case CLASS.EXECUTIONER:
                player2.anim = executionerAnim;
                break;
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString("classPlayer1", classPlayer1.ToString());
        PlayerPrefs.SetString("classPlayer2", classPlayer2.ToString());
    }

    private void LoadSave()
    {
        string classP1 = PlayerPrefs.GetString("classPlayer1");
        string classP2 = PlayerPrefs.GetString("classPlayer2");

        switch (classP1)
        {
            case "PRINCESS":
                classPlayer1 = CLASS.PRINCESS;
                break;
            case "KNIGHT":
                classPlayer1 = CLASS.KNIGHT;
                break;
            case "CLOWN":
                classPlayer1 = CLASS.CLOWN;
                break;
            case "EXECUTIONER":
                classPlayer1 = CLASS.EXECUTIONER;
                break;
        }
        switch (classP2)
        {
            case "PRINCESS":
                classPlayer2 = CLASS.PRINCESS;
                break;
            case "KNIGHT":
                classPlayer2 = CLASS.KNIGHT;
                break;
            case "CLOWN":
                classPlayer2 = CLASS.CLOWN;
                break;
            case "EXECUTIONER":
                classPlayer2 = CLASS.EXECUTIONER;
                break;
        }
    }


    public void Respawn(Player player)
    {

    }
}
