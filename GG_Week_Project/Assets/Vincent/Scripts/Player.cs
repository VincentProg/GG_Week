using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // ----------------------------------------- INPUTS

    public KeyCode keyMoveRight;
    public KeyCode keyMoveLeft;
    public KeyCode keyJump;
    public KeyCode keyDown;
    public KeyCode keyAttack;

    // ----------------------------------------- DEPLACEMENTS
    private bool canMove = true;
    private bool inversedSprite;
    [Header("RUN")]
    public float speed;
    [Header("JUMPS")]
    public float jumpForce;
    public float wallJumpForce;
    public float aerialControl;
    [HideInInspector]
    public bool isGrippingWall;
    private bool canRoulade = true;

    [Header("ROULADE")]
    public float accelerationRoulade;
    public float durationRoulade;
    private Collider2D playerCollider;
    private Collider2D rouladeChild;
    Coroutine rouladeCoroutine = null;

   
    
    private bool isDown = false;
    [Header("DOWN")]
    public float speedDown;
    [HideInInspector]
    public bool isCrouch = false;
    

    [HideInInspector]
    public bool grounded = true;
    [HideInInspector]
    public bool onWall = false;
    private Rigidbody2D rb;

    private float direction;



    // ---------------------------------------------------WEAPONS
    [HideInInspector]
    public enum WEAPON { PUNCH, SWORD, ARC, PIG }
    [Header("WEAPONS")]
    public WEAPON PlayerWeapon = WEAPON.PUNCH;

    private bool canAttack = true;
    public GameObject punch;
    public GameObject sword;
    public GameObject arc;
    public GameObject arrow;
    public float arrowSpeed;
    public float delayArc;
    public GameObject pig;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        rouladeChild = transform.GetChild(0).GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetKeyDown(keyMoveLeft))
            {
                direction--;
            }
            if (Input.GetKeyDown(keyMoveRight))
            {
                direction++;
            }
            if (Input.GetKeyUp(keyMoveLeft)){
                direction++;
            }
            if (Input.GetKeyUp(keyMoveRight))
            {
                direction--;
            }
            if (direction < 0)
            {
                transform.localScale = new Vector3(1, -1, 1);
                inversedSprite = true;
            }
            else if (direction > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                inversedSprite = false;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (grounded)
                {
                    Jump();
                    if (!canRoulade)
                    {
                        StopRoulade();
                    }
                    if(isGrippingWall){
                        isGrippingWall = false;
                    }
                    
                }

                if (!grounded && onWall)
                {
                    WallJump();
                    if (!canRoulade)
                    {
                        StopRoulade();
                    }
                    if (isGrippingWall)
                    {
                        isGrippingWall = false;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (grounded)
                {
                    if (Mathf.Abs(direction) > 0.6f)
                    {
                        print("roulade");
                        if (canRoulade)
                        {
                            rouladeCoroutine = StartCoroutine(Roulade(durationRoulade));
                        }
                        return;
                    }
                    else
                    {
                        GetDown();
                    }
                }

            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                if (isDown)
                {
                    GetUp();
                }
            }


        }

        if (isGrippingWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }


        if (Input.GetMouseButtonDown(0))
        {
            Attack();
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
        rb.velocity = Vector2.zero;
        Vector2 wallJump = new Vector2(-direction * wallJumpForce, wallJumpForce);
        rb.AddForce(wallJump, ForceMode2D.Impulse);
    }

    IEnumerator stopMoving(float delay)
    {

        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    IEnumerator Roulade(float duration)
    {
        isCrouch = true;
        canRoulade = false;
        rouladeChild.enabled = true;
        playerCollider.enabled = false;
        speed *= accelerationRoulade;
        GetComponent<SpriteRenderer>().enabled = false; // --------------------------------TEMPORARY
        rouladeChild.GetComponent<SpriteRenderer>().enabled = true;  // --------------------------------TEMPORARY
        yield return new WaitForSeconds(duration);
        speed /= accelerationRoulade;
        canRoulade = true;

        if (!Input.GetKey(KeyCode.LeftControl))
        {
         
            GetComponent<SpriteRenderer>().enabled = true;  // --------------------------------TEMPORARY
            rouladeChild.GetComponent<SpriteRenderer>().enabled = false;  // --------------------------------TEMPORARY
            rouladeChild.enabled = false;
            playerCollider.enabled = true;
        } else
        {
            GetDown();
        }
        print("roualdeEnd");
    }

    private void StopRoulade()
    {
        isCrouch = false;
        StopCoroutine(rouladeCoroutine);
        GetComponent<SpriteRenderer>().enabled = true;
        rouladeChild.GetComponent<SpriteRenderer>().enabled = false;
        speed /= accelerationRoulade;
        rouladeChild.enabled = false;
        playerCollider.enabled = true;
        canRoulade = true;
        print("StopRoulade");

    }

    private void GetDown()
    {
        isCrouch = true;
        isDown = true;
        speed *= speedDown;
        rouladeChild.enabled = true;
        playerCollider.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        rouladeChild.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void GetUp()
    {
        isCrouch = false;
        isDown = false;
        speed /= speedDown;
        rouladeChild.enabled = false;
        playerCollider.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        rouladeChild.GetComponent<SpriteRenderer>().enabled = false;
    }

    public IEnumerator GripWall()
    {
        isGrippingWall = true;
        yield return new WaitForSeconds(0.7f);
        isGrippingWall = false;
    }



    // --------------------------------------------------------- WEAPONS


    public void PickUpWeapon(WEAPON weapon)
    {
        if(weapon == WEAPON.SWORD)
        {
            PlayerWeapon = WEAPON.SWORD;
            return;
        }
        if(weapon == WEAPON.ARC)
        {
            PlayerWeapon = WEAPON.ARC;
            return;
        }
        if(weapon == WEAPON.PIG)
        {
            PlayerWeapon = WEAPON.PIG;
        }
    }

    public void LoseWeapon()
    {
        PlayerWeapon = WEAPON.PUNCH;
    }

    public void ThrowWeapon()
    {
        PlayerWeapon = WEAPON.PUNCH;
    }


    void Attack()
    {
        if (canAttack)
        {
            if (PlayerWeapon == WEAPON.PUNCH)
            {

            }
            if (PlayerWeapon == WEAPON.SWORD)
            {

            }
            if (PlayerWeapon == WEAPON.ARC)
            {

                StartCoroutine(DelayAttack(delayArc));
                int sens = 1;
                GameObject newArrow = Instantiate(arrow, arc.transform.position, arrow.transform.rotation);
                if (inversedSprite)
                {
                    sens = -1;
                    newArrow.transform.localScale = new Vector2(-1, newArrow.transform.localScale.y);
                }
                Vector2 arrowImpulse = new Vector2(sens * arrowSpeed, 0);
                newArrow.GetComponent<Rigidbody2D>().AddForce(arrowImpulse, ForceMode2D.Impulse);

            }
            if (PlayerWeapon == WEAPON.PIG)
            {

            }
        }
    }

    public void Death()
    {
        print("death");
    }

    IEnumerator DelayAttack(float delay)
    {
        print("delay");
        canAttack = false;
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }
}
