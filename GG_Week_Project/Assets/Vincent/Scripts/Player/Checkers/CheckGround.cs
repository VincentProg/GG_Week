using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    private Player parent;

    private void Start()
    {
        parent = transform.parent.GetComponent<Player>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), parent.GetComponent<Collider2D>());
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("grounded");
        parent.grounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("notGrounded");
        parent.grounded = false;
    }

}
