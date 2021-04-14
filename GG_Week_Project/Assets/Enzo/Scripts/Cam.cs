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

    public bool neutre = true;
    public bool DominantPlayer1 = false;

    private void Start()
    {
        Player1 = PlayerManager.instance.transformPlayer1;
        Player2 = PlayerManager.instance.transformPlayer2;
    }

    void Update()
    {

        
        Vector2 aimPosition;

        float distanceBothPlayer = new Vector2(Player1.position.x - Player2.position.x, 0).magnitude;
        if (distanceBothPlayer > 18.5f)
        {
            
        }

        if(neutre == true)
        {
            aimPosition = new Vector2((Player1.position.x + Player2.position.x)/2, transform.position.y);
            //transform.position = new Vector3((Player1.transform.position.x + Player2.transform.position.x) / 2,
            //                     transform.position.y,
            //                     transform.position.z);
            LeftCol.GetComponent<Collider2D>().isTrigger = false;
            RightCol.GetComponent<Collider2D>().isTrigger = false;
        }
        else if(DominantPlayer1 == true)
        {
            LeftCol.GetComponent<Collider2D>().isTrigger = true;
            RightCol.GetComponent<Collider2D>().isTrigger = false;
            aimPosition = new Vector2((Player1.position.x), transform.position.y);
            /*ransform.position = new Vector3(Player1.transform.position.x, transform.position.y, transform.position.z);*/
        }
        else
        {
            LeftCol.GetComponent<Collider2D>().isTrigger = false;
            RightCol.GetComponent<Collider2D>().isTrigger = true;
            aimPosition = new Vector2((Player2.position.x), transform.position.y);
            //transform.position = new Vector3(Player2.transform.position.x, transform.position.y, transform.position.z);
        }
        Vector3 direction = new Vector2(aimPosition.x - transform.position.x, 0);
        float distanceFromAim = direction.magnitude;
        if (distanceFromAim > 0.1f)
        transform.position = transform.position + direction.normalized * distanceFromAim * speedCamera * Time.deltaTime;

        
    }

    private void killNonDominant()
    {

    }
}

