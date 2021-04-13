using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCAC : MonoBehaviour
{

    private Collider2D col;
    

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(col, transform.parent.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().Death();
        }
    }


}
