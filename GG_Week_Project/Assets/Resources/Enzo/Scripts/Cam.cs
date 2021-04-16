using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private Transform Player1;
    private Transform Player2;

    
    public float speedCamera;


    public GameObject LeftCol;
    public GameObject RightCol;

    public GameObject Wall_P1;
    public GameObject Wall_P2;

    public bool canMove;


    private void Start()
    {
        Player1 = PlayerManager.instance.transformPlayer1;
        Player2 = PlayerManager.instance.transformPlayer2;
    }

    void Update()
    {

        if (canMove)
        {
            Vector2 aimPosition;


            if (PlayerManager.instance.neutral == true)
            {
                aimPosition = new Vector2((Player1.position.x + Player2.position.x) / 2, transform.position.y);
                //LeftCol.GetComponent<Collider2D>().isTrigger = false;
                //RightCol.GetComponent<Collider2D>().isTrigger = false;
            }
            else if (PlayerManager.instance.player1Dominant == true)
            {
                //LeftCol.GetComponent<Collider2D>().isTrigger = true;
                //RightCol.GetComponent<Collider2D>().isTrigger = false;
                aimPosition = new Vector2((Player1.position.x), transform.position.y);
            }
            else
            {
                //LeftCol.GetComponent<Collider2D>().isTrigger = false;
                //RightCol.GetComponent<Collider2D>().isTrigger = true;
                aimPosition = new Vector2((Player2.position.x), transform.position.y);
            }
            Vector3 direction = new Vector2(aimPosition.x - transform.position.x, 0);
            float distanceFromAim = direction.magnitude;
            if (distanceFromAim > 0.1f)
                transform.position = transform.position + direction.normalized * distanceFromAim * speedCamera * Time.deltaTime;

        }
    }

}

