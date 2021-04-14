using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCamera : MonoBehaviour
{
    public int playerToIgnore;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        if (playerToIgnore == 1)
            player = PlayerManager.instance.transformPlayer1;
        else
            player = PlayerManager.instance.transformPlayer2;


        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }



}
