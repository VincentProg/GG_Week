using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    public Collider2D colChild;
    [HideInInspector]
    public enum TYPE { PUNCH, SWORD, ARC, PIG }
    [Header("WEAPONS")]
    public TYPE thisWeapon;
    public bool canBeTaken = true;

    public Player owner;
    private bool isAttacking;
    private bool canChangePos = true;
    private Transform transformAim;
    private Vector2 vectorAim;
    public Transform posAttack1;
    public Transform posAttack2;
    public float speed;
    public float speedAttack;
    public float rangeAttack;
    private float timeSave;

    [SerializeField]
    private LayerMask playerLayer;
    public bool colliderMortel;



    public List<Transform> Pos = new List<Transform>();

    public Sprite swordSprite;
    public Sprite arcSprite;
    public Sprite pigSprite;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        switch (thisWeapon)
        {
            case TYPE.SWORD:
                GetComponent<SpriteRenderer>().sprite = swordSprite;
                break;
            case TYPE.ARC:
                GetComponent<SpriteRenderer>().sprite = arcSprite;
                break;
            case TYPE.PIG:
                GetComponent<SpriteRenderer>().sprite = pigSprite;
                break;
        }

        Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.transform.GetChild(0).GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.transform.GetChild(0).GetComponent<Collider2D>());


    }


    private void Update()
    {
        if (canBeTaken && rb.velocity.magnitude >= 0.5f)
        {
            canBeTaken = false;
        } else
        {
            canBeTaken = true;
        }


        if (isAttacking)
        {
            MiddleAttack();
        }
        
    }

    private void FixedUpdate()
    {
        if (owner != null)
        {
            if (!isAttacking)
            {
                if (canChangePos)
                {
                    StartCoroutine(ChangePosAim());

                }
                if (transform.position.x - posAttack1.position.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else transform.localScale = new Vector3(-1, 1, 1);

                Vector3 direction = new Vector2(transformAim.position.x - transform.position.x, transformAim.position.y - transform.position.y);
                float distanceFromAim = direction.magnitude;
                transform.position = transform.position + direction.normalized * distanceFromAim * speed * Time.fixedDeltaTime;
            }
            else
            {

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (owner == null && colliderMortel)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.GetComponent<Player>().TakeDamages(3);
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            }

            rb.gravityScale = 1;
        }

        colliderMortel = false;
        

        Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.transform.GetChild(0).GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.transform.GetChild(0).GetComponent<Collider2D>());

    }

    public void StartAttack()
    {
        isAttacking = true;
        if (!owner.isCrouch)
        {
            vectorAim = posAttack1.position;
        }
        else
        {
            vectorAim = posAttack2.position;
        }

        //if(owner.id == 1)
        //{

        //    Physics2D.IgnoreCollision(colChild, PlayerManager.instance.player2.GetComponent<Collider2D>(), false);
        //    Physics2D.IgnoreCollision(colChild, PlayerManager.instance.player2.transform.GetChild(0).GetComponent<Collider2D>(), false);

        //} else
        //{
        //    Physics2D.IgnoreCollision(colChild, PlayerManager.instance.player1.GetComponent<Collider2D>(), false);
        //    Physics2D.IgnoreCollision(colChild, PlayerManager.instance.player1.transform.GetChild(0).GetComponent<Collider2D>(), false);
        //}
        timeSave = Time.time;

    }

    private void MiddleAttack()
    {
        float time = (Time.time - timeSave) * speedAttack ;
        transform.position = Vector2.Lerp(transform.position, vectorAim, time);
        if(time > 0.1f)
        {
            ThrowRaycast();
        }

        if(time >= 1)
        {
            isAttacking = false;
            
        }

        //if (owner.id == 1)
        //{

        //    Physics2D.IgnoreCollision(colChild, PlayerManager.instance.player2.GetComponent<Collider2D>());
        //    Physics2D.IgnoreCollision(colChild, PlayerManager.instance.player2.transform.GetChild(0).GetComponent<Collider2D>());
        //}
        //else
        //{
        //    Physics2D.IgnoreCollision(colChild, PlayerManager.instance.player1.GetComponent<Collider2D>());
        //    Physics2D.IgnoreCollision(colChild, PlayerManager.instance.player1.transform.GetChild(0).GetComponent<Collider2D>());
        //}
    }
    

    IEnumerator ChangePosAim()
    {
        canChangePos = false;
        int random = Random.Range(0, Pos.Count);
        transformAim = Pos[random];
        print(transformAim);
        float randTime = Random.Range(0.8f, 1.5f);
        yield return new WaitForSeconds(randTime);
        canChangePos = true;
    }


    void ThrowRaycast()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.right, rangeAttack, playerLayer);
        
        foreach(RaycastHit2D hit2D in hit)
        {
            Player playerHit = hit2D.transform.GetComponent<Player>();
            if(playerHit != null && owner.id != playerHit.id)
            {
                playerHit.TakeDamages(3);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.right * rangeAttack);
    }
}
