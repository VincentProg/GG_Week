using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movements")]
    public float speed;
    public float jumpForce;
    public float wallJumpForce;
    public float aerialControl;
    private bool canMove = true;

    [HideInInspector]
    public bool grounded = true;
    [HideInInspector]
    public bool onWall = false;
    private Rigidbody2D rb;

    private float direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            direction = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                if (grounded)
                {
                    Jump();
                }

                if (!grounded && onWall)
                {
                    WallJump();
                    print("walljump");
                }
            }
        }

        
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (grounded)
            {
                float Xvelocity = direction * speed;
                rb.velocity = new Vector2(Xvelocity, rb.velocity.y);
            } else
            {
                float Xvelocity = direction * speed;
                float currentVelocity = rb.velocity.x;

                float newXvelocity = currentVelocity + (Xvelocity * aerialControl);
                newXvelocity = Mathf.Clamp(newXvelocity, -speed, speed);
                rb.velocity = new Vector2(newXvelocity, rb.velocity.y);

            }
        }
    }

    private void Jump()
    {       
        Vector2 jump = new Vector2(0, jumpForce);
        rb.AddForce(jump, ForceMode2D.Impulse);        
    }

    private void WallJump()
    {
        Vector2 wallJump = new Vector2(-direction * wallJumpForce, wallJumpForce);
        rb.AddForce(wallJump, ForceMode2D.Impulse);
    }

    IEnumerator stopMoving(float delay)
    {

        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;
    }
}
