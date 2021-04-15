using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    [HideInInspector]
    public enum TYPE { PUNCH, SWORD, ARC, PIG }
    [Header("WEAPONS")]
    public TYPE thisWeapon;
    public bool canBeTaken = true;

    public Sprite swordSprite;
    public Sprite arcSprite;
    public Sprite pigSprite;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        //switch (thisWeapon)
        //{
        //    case TYPE.SWORD:
        //        GetComponent<SpriteRenderer>().sprite = swordSprite;
        //        break;
        //    case TYPE.ARC:
        //        GetComponent<SpriteRenderer>().sprite = arcSprite;
        //        break;
        //    case TYPE.PIG:
        //        GetComponent<SpriteRenderer>().sprite = pigSprite;
        //        break;
        //}
    }


    private void Update()
    {
        if (canBeTaken && rb.velocity.magnitude >= 0.5f)
        {
            canBeTaken = false;
        } else
        {
            canBeTaken = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            
            collision.transform.GetComponent<Player>().TakeDamages(3);
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }

        rb.gravityScale = 1;
        
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.transform.GetChild(0).GetComponent<Collider2D>());
        if (PlayerManager.instance.player2 != null)
        {
            Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.transform.GetChild(0).GetComponent<Collider2D>());
        }
    }
}
