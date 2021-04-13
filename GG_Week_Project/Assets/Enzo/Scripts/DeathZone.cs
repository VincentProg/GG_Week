using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    bool death = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        death = true;
        Debug.Log("T'es mort !");
        Destroy(col.gameObject);
    }
}
