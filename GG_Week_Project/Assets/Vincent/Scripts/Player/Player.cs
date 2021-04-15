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
    public KeyCode keyThrow;

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
    public Transform checkground;
    [HideInInspector]
    public bool onWall = false;
    private Rigidbody2D rb;

    private float direction;



    // ---------------------------------------------------WEAPONS
    [Header("WEAPONS")]
    public Weapon.TYPE PlayerWeapon = Weapon.TYPE.PUNCH;

    private bool canAttack = true;
    //public GameObject punch;
    //public GameObject sword;
    //public GameObject arc;
    public GameObject arrow;
    public float arrowSpeed;
    public float delayArc;
    public GameObject pig;
    public float throwStrength;

    //public GameObject swordPrefab;
    //public GameObject arcPrefab;
    //public GameObject pigPrefab;

    public Transform myWeaponTransform;
    public Weapon myWeapon;
    public List<Transform> posWeapon = new List<Transform>();
    public Transform posWeaponAttack1;
    public Transform posWeaponattack2;

    // ------------------------------------------------- HEALTH

    public bool isDead = false;
    public int health = 3;

    // ------------------------------------------------- INSTANCES
    public int id;


    // -------------------------------------------------- ANIMATIONS
    public Animator anim;

    // Start is called before the first frame update

    private void Awake()
    {
        if (id == 1)
        {
            PlayerManager.instance.player1 = this;
            PlayerManager.instance.transformPlayer1 = transform;
        }
        else
        {
            PlayerManager.instance.player2 = this;
            PlayerManager.instance.transformPlayer2 = transform;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        rouladeChild = transform.GetChild(0).GetComponent<Collider2D>();
        //Physics2D.IgnoreCollision(myWeaponTransform.GetComponent<Collider2D>(), PlayerManager.instance.player1.GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(myWeaponTransform.GetComponent<Collider2D>(), PlayerManager.instance.player1.transform.GetChild(0).GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(myWeaponTransform.GetComponent<Collider2D>(), PlayerManager.instance.player2.GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(myWeaponTransform.GetComponent<Collider2D>(), PlayerManager.instance.player2.transform.GetChild(0).GetComponent<Collider2D>());
        myWeapon.Pos = posWeapon;
        myWeapon.posAttack1 = posWeaponAttack1;
        myWeapon.posAttack2 = posWeaponattack2;
        myWeapon.owner = this;
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
                transform.localScale = new Vector3(1, 1, 1);
                inversedSprite = true;
            }
            else if (direction > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
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
                        if (canRoulade)
                        {
                            rouladeCoroutine = StartCoroutine(Roulade(durationRoulade));
                        }
                    }
                    else
                    {
                        GetDown();
                    }

                    PickUpWeapon();

                }
            }

            if (Input.GetKeyUp(keyDown))
            {
                if (isDown)
                {
                    GetUp();
                }

               
            }

            if (Input.GetKeyDown(keyThrow))
            {
                ThrowWeapon();
            }


        }

        if (isGrippingWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }


        if (Input.GetKeyDown(keyAttack))
        {
            if(myWeapon != null)
            myWeapon.StartAttack();
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position - new Vector3(0,0.5f), transform.position + (Vector3.right * 0.8f) - new Vector3(0, 0.5f));
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
        RaycastHit2D hit = Physics2D.Raycast(checkground.position, Vector2.down, 0.2f, 1<<6);
        if (hit)
        {
            grounded = true;
        } else
        {
            grounded = false;
        }

        Debug.DrawLine(checkground.position, checkground.position + Vector3.down * 0.2f);
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



    // ------------------------------------------------------------------------------------------------------------------- WEAPONS




    public void PickUpWeapon()
    {
        
        if (myWeapon == null) // ------------ PICK UP WEAPON
        {
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, 0.8f), 0.5f);
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].GetComponent<Weapon>() && objects[i].GetComponent<Weapon>().owner == null)
                {
                    myWeaponTransform = objects[i].transform;
                    myWeapon = objects[i].GetComponent<Weapon>() ;
                    myWeapon.owner = this;
                    myWeapon.Pos = posWeapon;
                    myWeapon.posAttack1 = posWeaponAttack1;
                    myWeapon.posAttack2 = posWeaponattack2;
                    myWeaponTransform.GetComponent<Rigidbody2D>().gravityScale = 0;
                    print("pickUp");
                    return;
                }
            }
        }
        
        
    }

    public void LoseWeapon()
    {
        PlayerWeapon = Weapon.TYPE.PUNCH;
    }

    public void ThrowWeapon()
    {
        if (myWeapon != null)
        {
            myWeapon.colliderMortel = true;

            if (id == 1)
            {
                Physics2D.IgnoreCollision(myWeaponTransform.GetComponent<Collider2D>(), PlayerManager.instance.player2.GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(myWeaponTransform.GetComponent<Collider2D>(), PlayerManager.instance.player2.transform.GetChild(0).GetComponent<Collider2D>(), false);
            } else
            {
                Physics2D.IgnoreCollision(myWeaponTransform.GetComponent<Collider2D>(), PlayerManager.instance.player1.GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(myWeaponTransform.GetComponent<Collider2D>(), PlayerManager.instance.player1.transform.GetChild(0).GetComponent<Collider2D>(), false);
            }
            myWeapon.GetComponent<Rigidbody2D>().AddForce(new Vector2(-transform.localScale.x * throwStrength, 0), ForceMode2D.Impulse);
            myWeapon.owner = null;
            myWeapon = null;
            myWeaponTransform = null;
        

        }

        
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

                //StartCoroutine(DelayAttack(delayArc));
                //int sens = 1;
                //GameObject newArrow = Instantiate(arrow, arc.transform.position, arrow.transform.rotation);
                //if (inversedSprite)
                //{
                //    sens = -1;
                //    newArrow.transform.localScale = new Vector2(-1, newArrow.transform.localScale.y);
                //    print("bug");
                //}
                //Vector2 arrowImpulse = new Vector2(sens * arrowSpeed, 0);
                //newArrow.GetComponent<Rigidbody2D>().AddForce(arrowImpulse, ForceMode2D.Impulse);
                //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), newArrow.GetComponent<Collider2D>());

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
        isDead = true;
        if (myWeapon != null)
        {
            myWeaponTransform.GetComponent<Rigidbody2D>().gravityScale = 1;
            myWeapon.owner = null;
            myWeapon = null;
            myWeaponTransform = null;
        }


        gameObject.SetActive(false);
        PlayerManager.instance.DeathPlayer(this);
    }

    public void ResetDirection()
    {
        direction = 0;
        if (Input.GetKey(keyMoveRight))
        {
            direction++;
        }
        if (Input.GetKey(keyMoveLeft))
        {
            direction--;
        }
    }

    IEnumerator DelayAttack(float delay)
    {
        canAttack = false;
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }

    void FonctionAnimation()
    {
        Debug.Log("Animation fonction");
    }

}
