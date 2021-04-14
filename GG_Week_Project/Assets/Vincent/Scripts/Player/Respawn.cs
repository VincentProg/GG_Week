using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    public bool player1;

    // Start is called before the first frame update
    void Start()
    {
        if (player1) 
        {
            PlayerManager.instance.respawnP1 = transform;
        } else
        {
            PlayerManager.instance.respawnP2 = transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
