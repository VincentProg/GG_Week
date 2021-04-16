using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] Transform SpawnPlayer1;
    [SerializeField] Transform SpawnPlayer2;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player1"))
        {
            col.transform.position = SpawnPlayer1.position;
        }
        if (col.transform.CompareTag("Player2"))
        {
            col.transform.position = SpawnPlayer2.position;
        }
    }
}
