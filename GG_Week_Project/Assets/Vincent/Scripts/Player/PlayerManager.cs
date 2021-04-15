using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;

    [HideInInspector]
    public Player player1;
    [HideInInspector]
    public Player player2;

    [HideInInspector]
    public Transform transformPlayer1;
    [HideInInspector]
    public Transform transformPlayer2;

    private Cam cameraScene;
    public GameObject wallBeforeKillL;
    public GameObject wallBeforeKillR;

    public GameObject arrowDirectionP1;
    public GameObject arrowDirectionP2;

    //[HideInInspector]
    //public enum WEAPON { PUNCH, SWORD, ARC, PIG }

    public enum CLASS { PRINCESS, KNIGHT, CLOWN, EXECUTIONER}


    public CLASS classPlayer1;
    public CLASS classPlayer2;

    public Animator princessAnim;
    public Animator knightAnim;
    public Animator clownAnim;
    public Animator executionerAnim;


    public Transform respawnP1_1;
    public Transform respawnP1_2;
    public Transform respawnP1_3;
    public Transform respawnP2_1;
    public Transform respawnP2_2;
    public Transform respawnP2_3;

    public bool neutral;
    public bool player1Dominant;

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

        cameraScene = FindObjectOfType<Cam>();
        
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


    public void DeathPlayer(Player player)
    {
        if(player == player1)
        {
            neutral = false;
            player1Dominant = false;
            arrowDirectionP1.SetActive(false);
            arrowDirectionP2.SetActive(true);
        } else
        {
            neutral = false;
            player1Dominant = true;
            arrowDirectionP1.SetActive(true);
            arrowDirectionP2.SetActive(false);
        }
        StartCoroutine(RespawnPlayer(player));
        cameraScene.canMove = true;
        wallBeforeKillL.SetActive(false);
        wallBeforeKillR.SetActive(false);
    }

    IEnumerator RespawnPlayer(Player player)
    {
        yield return new WaitForSeconds(2);
        if(player == player1)
        {
            Vector2 distP2_R1_1 = new Vector2(respawnP1_1.position.x - transformPlayer2.position.x, 0);
            Vector2 distP2_R1_2 = new Vector2(respawnP1_2.position.x - transformPlayer2.position.x, 0);
            print(distP2_R1_1);
            if(distP2_R1_1.x < -5)
            {
                transformPlayer1.position = respawnP1_1.position;
            } else if(distP2_R1_2.x < -5)
            {
                transformPlayer1.position = respawnP1_2.position;
            } else
            {
                transformPlayer1.position = respawnP1_3.position;
            }
            transformPlayer1.gameObject.SetActive(true);
            player1.isDead = false;
            player1.ResetDirection();
        } else
        {
            Vector2 distP1_R2_1 = new Vector2(respawnP2_1.position.x - transformPlayer1.position.x, 0);
            Vector2 distP1_R2_2 = new Vector2(respawnP2_2.position.x - transformPlayer1.position.x, 0);
            print(distP1_R2_1);
            if (distP1_R2_1.x > 5)
            {
                transformPlayer2.position = respawnP2_1.position;
            }
            else if(distP1_R2_2.x > 5)
            {
                transformPlayer2.position = respawnP2_2.position;
            } else
            {
                transformPlayer2.position = respawnP2_3.position;
            }
            transformPlayer2.gameObject.SetActive(true);
            player2.isDead = false;
            player2.ResetDirection();
        }

        neutral = true;

        cameraScene.Wall_P1.SetActive(false);
        cameraScene.Wall_P2.SetActive(false);
        yield return new WaitForSeconds(2);
        cameraScene.Wall_P1.SetActive(true);
        cameraScene.Wall_P2.SetActive(true);

    }
}
