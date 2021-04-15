using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCameraTrigger : MonoBehaviour
{
    public int playerToKill;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        if (playerToKill == 1)
            player = PlayerManager.instance.transformPlayer1;
        else
            player = PlayerManager.instance.transformPlayer2;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform == PlayerManager.instance.transformPlayer1)
        {
            if (playerToKill == 1 && !PlayerManager.instance.player1Dominant)
            {
                PlayerManager.instance.player1.Death();
            }

        }
        else if (collision.transform == PlayerManager.instance.transformPlayer2)
        {
            if (playerToKill == 2 && PlayerManager.instance.player1Dominant)
            {
                PlayerManager.instance.player2.Death();
            }
        }
    }
}
