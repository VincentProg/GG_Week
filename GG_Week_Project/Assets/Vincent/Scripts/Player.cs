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
    [Header("WEAPONS")]
    public Weapon.TYPE PlayerWeapon = Weapon.TYPE.PUNCH;

    private bool canAttack = true;
    public GameObject punch;
    public GameObject sword;
    public GameObject arc;
    public GameObject arrow;
    public float arrowSpeed;
    public float delayArc;
    public GameObject pig;
    private Animator anim;


    public List<Weapon> weaponsNear = new List<Weapon>();
    // ------------------------------------------------- HEALTH

    private bool isDead = false;
    private int health = 3;

    // ------------------------------------------------- INSTANCES
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerManager.instance.player1 == null)
        {
            PlayerManager.instance.player1 = this;
            id = 1;
        } else 
        {
            PlayerManager.instance.player2 = this;
            id = 2;
        }


        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        rouladeChild = transform.GetChild(0).GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
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

            if (Input.GetKeyDown(keyJump))
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

            if (Input.GetKeyDown(keyDown))
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
                    }
                    else
                    {
                        GetDown();
                    }

                    if (PlayerWeapon == Weapon.TYPE.PUNCH) // ------------ PICK UP WEAPON
                    {
                        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, 0.5f), 0.3f);
                        for (int i = 0; i < objects.Length; i++)
                        {
                            if (objects[i].GetComponent<Weapon>())
                            {
                                PickUpWeapon(objects[i].GetComponent<Weapon>().thisWeapon);
                                Destroy(objects[i].gameObject);
                            }
                        }




                    }
                }

                

            }

            if (Input.GetKeyUp(keyDown))
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position - new Vector3(0,0.5f), transform.position + (Vector3.right * 0.3f) - new Vector3(0, 0.5f));
    }

    private void FixedUpdate()
    {
        if (!isDead)
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




    public void PickUpWeapon(Weapon.TYPE weapon)
    {


        if(weapon == Weapon.TYPE.SWORD)
        {
            PlayerWeapon = Weapon.TYPE.SWORD;
            return;
        }
        if(weapon == Weapon.TYPE.ARC)
        {
            PlayerWeapon = Weapon.TYPE.ARC;
            return;
        }
        if(weapon == Weapon.TYPE.PIG)
        {
            PlayerWeapon = Weapon.TYPE.PIG;
        }
    }

    public void LoseWeapon()
    {
        PlayerWeapon = Weapon.TYPE.PUNCH;
    }

    public void ThrowWeapon()
    {
        PlayerWeapon = Weapon.TYPE.PUNCH;
    }


    void Attack()
    {
        if (canAttack)
        {
            if (PlayerWeapon == Weapon.TYPE.PUNCH)
            {

            }
            if (PlayerWeapon == Weapon.TYPE.SWORD)
            {

            }
            if (PlayerWeapon == Weapon.TYPE.ARC)
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
            if (PlayerWeapon == Weapon.TYPE.PIG)
            {

            }
        }
    }

    public void TakeDamages(int damages)
    {
        health -= damages;
        if(health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        print("death");
        isDead = true;
    }

    IEnumerator DelayAttack(float delay)
    {
        print("delay");
        canAttack = false;
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }
}
