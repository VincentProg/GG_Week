using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;

    public Player player1;
    public Player player2;

    [HideInInspector]
    public enum WEAPON { PUNCH, SWORD, ARC, PIG }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }



}
