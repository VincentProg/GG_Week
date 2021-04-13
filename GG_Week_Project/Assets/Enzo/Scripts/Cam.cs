using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject LeftCol;
    public GameObject RightCol;

    public bool neutre = true;
    public bool DominantPlayer1 = false;
    public bool DominantPlayer2 = false;

    void Update()
    {
        if(neutre == true)
        {
            transform.position = new Vector3((Player1.transform.position.x + Player2.transform.position.x) / 2,
                                 transform.position.y,
                                 transform.position.z);
            LeftCol.GetComponent<Collider2D>().isTrigger = false;
            RightCol.GetComponent<Collider2D>().isTrigger = false;
        }
        else if(DominantPlayer1 == true)
        {
            LeftCol.GetComponent<Collider2D>().isTrigger = true;
            RightCol.GetComponent<Collider2D>().isTrigger = false;
            transform.position = new Vector3(Player1.transform.position.x, transform.position.y, transform.position.z);
        }else if(DominantPlayer2 == true)
        {
            LeftCol.GetComponent<Collider2D>().isTrigger = false;
            RightCol.GetComponent<Collider2D>().isTrigger = true;
            transform.position = new Vector3(Player2.transform.position.x, transform.position.y, transform.position.z);
        }

    }
}

