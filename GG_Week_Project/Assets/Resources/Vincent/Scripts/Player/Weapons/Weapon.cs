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
    private bool isAttackLerp;
    private bool isAttacking;
    Vector2 posStartLerp;
    private bool canChangePos = true;
    private Transform transformAim;
    private Vector2 vectorFollow;
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

    bool reverse;
    public Sprite swordSprite;
    public Sprite arcSprite;
    public Sprite pigSprite;


    public GameObject arrow;
    public float arrowSpeed;

    private Vector2 directionToEnemy;
    private Vector2 vectorAim;

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
        if (PlayerManager.instance.player1 != null)
        {
            Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.transform.GetChild(0).GetComponent<Collider2D>());
        }
        if (PlayerManager.instance.player2 != null)
        {
            Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.transform.GetChild(0).GetComponent<Collider2D>());
        }


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


        if (isAttackLerp)
        {
            if(thisWeapon == TYPE.SWORD)
            LERPAttack();
            else if(thisWeapon == TYPE.PIG)
            {
                LERPAttack();
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (owner != null)
        {
            if (!isAttacking)
            {
                if (thisWeapon == TYPE.SWORD || thisWeapon == TYPE.ARC)
                {
                    if (canChangePos)
                    {
                        StartCoroutine(ChangePosAim());

                    }
                    if (transform.position.x - posAttack1.position.x < 0)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        reverse = false;
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        reverse = true;
                    }

                    Vector3 direction = new Vector2(transformAim.position.x - transform.position.x, transformAim.position.y - transform.position.y);
                    float distanceFromAim = direction.magnitude;
                    transform.position = transform.position + direction.normalized * distanceFromAim * speed * Time.fixedDeltaTime;
                    return;
                }

                else
                {
                    if (!isAttacking)
                    {
                        vectorFollow = new Vector2(owner.transform.position.x, owner.transform.position.y) + new Vector2(0, 3);

                        Vector3 direction = new Vector2(vectorFollow.x - transform.position.x, vectorFollow.y - transform.position.y);
                        float distanceFromAim = direction.magnitude;
                        transform.position = transform.position + direction.normalized * distanceFromAim * speed * Time.fixedDeltaTime;

                        if (owner.id == 1)
                        {
                            directionToEnemy = new Vector2(PlayerManager.instance.transformPlayer2.position.x - transform.position.x, PlayerManager.instance.transformPlayer2.position.y - transform.position.y);
                        }
                        else
                        {
                            directionToEnemy = new Vector2(PlayerManager.instance.transformPlayer1.position.x - transform.position.x, PlayerManager.instance.transformPlayer1.position.y - transform.position.y);
                        }
                        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(directionToEnemy.y, directionToEnemy.x));
                        return;
                    }
                }
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

        if (PlayerManager.instance.player1 != null)
        {
            Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(col, PlayerManager.instance.player1.transform.GetChild(0).GetComponent<Collider2D>());
        }
        if (PlayerManager.instance.player2 != null)
        {
            Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(col, PlayerManager.instance.player2.transform.GetChild(0).GetComponent<Collider2D>());
        }
        

    }

    public void StartAttack()
    {
        if (thisWeapon == TYPE.SWORD)
        {
            isAttackLerp = true;
            isAttacking = true;
            if (!owner.isCrouch)
            {
                vectorFollow = posAttack1.position;
            }
            else
            {
                vectorFollow = posAttack2.position;
            }

            timeSave = Time.time;
            AudioManager.instance.Play("Slash");
            vectorAim = vectorFollow;
            posStartLerp = transform.position;

        } else if(thisWeapon == TYPE.ARC)
        {
            int sens = 1;
            GameObject newArrow = Instantiate(arrow, transform.position, arrow.transform.rotation);
            if (reverse)
            {
                sens = -1;
                newArrow.transform.localScale = new Vector2(-1, newArrow.transform.localScale.y);
                print("bug");
            }
            Vector2 arrowImpulse = new Vector2(sens * arrowSpeed, 0);
            newArrow.GetComponent<Rigidbody2D>().AddForce(arrowImpulse, ForceMode2D.Impulse);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), newArrow.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(owner.transform.GetComponent<Collider2D>(), newArrow.GetComponent<Collider2D>());
        } else
        {
            if(owner.id == 1)
            {
                vectorAim = PlayerManager.instance.transformPlayer2.position;
            } else vectorAim = PlayerManager.instance.transformPlayer1.position;
            isAttacking = true;
            StartCoroutine(AttackSpear());
        }

    }

    private void LERPAttack()
    {
        float time = (Time.time - timeSave) * speedAttack ;
        transform.position = Vector2.Lerp(posStartLerp, vectorAim, time);
        ThrowRaycast();


        if(time >= 1)
        {
            isAttackLerp = false;
            isAttacking = false;
        }

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


    IEnumerator AttackSpear()
    {
        rb.velocity = -directionToEnemy.normalized * 4;
        yield return new WaitForSeconds(0.5f);
        timeSave = Time.time;
        isAttackLerp = true;
        posStartLerp = transform.position;
    }

}
