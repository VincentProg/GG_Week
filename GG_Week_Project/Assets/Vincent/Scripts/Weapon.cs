using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Rigidbody2D rb;
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
}
