using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private Rigidbody2D rb; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        print("arrow");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            //transform.parent = collision.transform;
            Destroy(gameObject);
            collision.transform.GetComponent<Player>().Death();
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
