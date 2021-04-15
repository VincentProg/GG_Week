using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{

    private Collider2D col;
    public int damages;


    private void Start()
    {
        col = GetComponent<Collider2D>();
        transform.parent.GetComponent<Weapon>().colChild = col;

        Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.transform.GetChild(0).GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.transform.GetChild(0).GetComponent<Collider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("damages");

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamages(damages);
        }
    }


}
