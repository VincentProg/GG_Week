using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWall : MonoBehaviour
{
    private Player parent;

    private void Start()
    {
        parent = transform.parent.GetComponent<Player>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), parent.GetComponent<Collider2D>());

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        parent.onWall = true;
        parent.StartCoroutine("GripWall");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        parent.isGrippingWall = false;
        parent.onWall = false;
    }

}
