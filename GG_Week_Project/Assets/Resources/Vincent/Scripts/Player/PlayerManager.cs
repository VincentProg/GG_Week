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

    public GameObject Sword;
    public GameObject Arc;
    public GameObject Spear;

    public Weapon.TYPE weaponScene;


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

    public List<Transform> respawnsP1 = new List<Transform>();
    public List<Transform> respawnsP2 = new List<Transform>();

    public bool neutral;
    public bool player1Dominant;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
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
        if (!neutral)
        {
            wallBeforeKillL.SetActive(false);
            wallBeforeKillR.SetActive(false);
            cameraScene.canMove = true;

            if (player1Dominant)
            {
                arrowDirectionP1.SetActive(true);
            }
            else arrowDirectionP2.SetActive(true);
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
        yield return new WaitForSeconds(1);
        if (player == player1)
        {

            for (int i = 0; i < respawnsP1.Count; i++)
            {
                Vector2 distance = new Vector2(respawnsP1[i].position.x - transformPlayer2.position.x, 0);
                if (distance.x < -5)
                {
                    transformPlayer1.position = respawnsP1[i].position;
                    break;
                }
                else transformPlayer1.position = respawnsP1[respawnsP1.Count - 1].position;
            }
            transformPlayer1.gameObject.SetActive(true);
            player1.isDead = false;
            player1.ResetDirection();
            if (weaponScene == Weapon.TYPE.SWORD)
            {
                GameObject newSword = Instantiate(Sword, transformPlayer1.position, transform.rotation);
                Weapon script = newSword.GetComponent<Weapon>();
                script.thisWeapon = Weapon.TYPE.SWORD;
                script.owner = player1;
                player1.myWeapon = script;
                player1.myWeaponTransform = newSword.transform;
                script.Pos = player1.posWeapon;
                script.posAttack1 = player1.posWeaponAttack1;
                script.posAttack2 = player1.posWeaponattack2;
            }
            else if (weaponScene == Weapon.TYPE.ARC)
            {
                GameObject newArc = Instantiate(Arc, transformPlayer1.position, transform.rotation);
                Weapon script = newArc.GetComponent<Weapon>();
                script.thisWeapon = Weapon.TYPE.ARC;
                script.owner = player1;
                player1.myWeapon = script;
                player1.myWeaponTransform = newArc.transform;
                script.Pos = player1.posWeapon;
                script.posAttack1 = player1.posWeaponAttack1;
                script.posAttack2 = player1.posWeaponattack2;
            }
            else
            {
                GameObject newArc = Instantiate(Spear, transformPlayer1.position, transform.rotation);
                Weapon script = newArc.GetComponent<Weapon>();
                script.thisWeapon = Weapon.TYPE.PIG;
                script.owner = player1;
                player1.myWeapon = script;
                player1.myWeaponTransform = newArc.transform;
                //script.Pos = player1.posWeapon;
                //script.posAttack1 = player1.posWeaponAttack1;
                //script.posAttack2 = player1.posWeaponattack2;
            }
        }
        else
        {
            for (int i = 0; i < respawnsP2.Count; i++)
            {
                Vector2 distance = new Vector2(respawnsP2[i].position.x - transformPlayer1.position.x, 0);
                if (distance.x > 5)
                {
                    transformPlayer2.position = respawnsP2[i].position;
                    break;
                }
                else transformPlayer2.position = respawnsP2[respawnsP1.Count - 1].position;
            }
            transformPlayer2.gameObject.SetActive(true);
            player2.isDead = false;
            player2.ResetDirection();

            if (weaponScene == Weapon.TYPE.SWORD)
            {
                GameObject newSword = Instantiate(Sword, transformPlayer2.position, transform.rotation);
                Weapon script = newSword.GetComponent<Weapon>();
                script.thisWeapon = Weapon.TYPE.SWORD;
                script.owner = player2;
                player2.myWeapon = script;
                player2.myWeaponTransform = newSword.transform;
                script.Pos = player2.posWeapon;
                script.posAttack1 = player2.posWeaponAttack1;
                script.posAttack2 = player2.posWeaponattack2;

            }
            else if (weaponScene == Weapon.TYPE.ARC)
            {
                GameObject newArc = Instantiate(Arc, transformPlayer2.position, transform.rotation);
                Weapon script = newArc.GetComponent<Weapon>();
                script.thisWeapon = Weapon.TYPE.ARC;
                script.owner = player2;
                player2.myWeapon = script;
                player2.myWeaponTransform = newArc.transform;
                script.Pos = player2.posWeapon;
                script.posAttack1 = player2.posWeaponAttack1;
                script.posAttack2 = player2.posWeaponattack2;
            }
            else
            {
                GameObject newArc = Instantiate(Spear, transformPlayer2.position, transform.rotation);
                Weapon script = newArc.GetComponent<Weapon>();
                script.thisWeapon = Weapon.TYPE.PIG;
                script.owner = player2;
                player2.myWeapon = script;
                player2.myWeaponTransform = newArc.transform;
            }
        }

        neutral = true;

        cameraScene.Wall_P1.SetActive(false);
        cameraScene.Wall_P2.SetActive(false);
        yield return new WaitForSeconds(2);
        cameraScene.Wall_P1.SetActive(true);
        cameraScene.Wall_P2.SetActive(true);

    }
}
