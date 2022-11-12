using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAI : EnemyAI
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float attackingTurnSpeed;
    [SerializeField] private float alertRadius;     // how fast enemy turns towards the player
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRange;
    [SerializeField] private float rangedAttackDelay;
    [SerializeField] private float rangedAttackRange;
    [SerializeField] private float rangedAttackForce;
    [SerializeField] private float rangedAttackUpForce;
    [SerializeField] private float rangedAttackAngle;
    [SerializeField] private Transform damageBox;
    [SerializeField] private Transform boulder;
    [SerializeField] private Transform boulderSpawn;
    private Transform player;
    private bool active;
    private bool falling;
    private Rigidbody rb;
    private float extraGravity = 2000;
    private float timer;            // for attack to initiate
    private bool attacking;         // attack has started
    private bool ranged;         // attack has started

    private bool canMove;           // there is space in front of the enemy to move
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        active = true;
        canMove = true;
        ranged = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        int layerMask = 1 << 3;

        rb.AddForce(Vector3.down * Time.deltaTime * extraGravity);      // Extra Gravity
        Vector3 relLoc = player.position - transform.position;
        anim.SetBool("Walking", false);

        if (active && relLoc.magnitude < alertRadius)
        {
            if (!attacking)
            {
                if (Physics.SphereCast(transform.position, 1, transform.forward, out hit, attackRange, layerMask) && Time.time > timer)
                {
                    anim.SetTrigger("Attack");
                    attacking = true;
                    timer = Time.time + attackDelay;
                }

                // Movement
                relLoc.y = 0;
                if (canMove)
                {
                    transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
                    anim.SetBool("Walking", true);
                }
                else if (!attacking)    // Checks if it can do a ranged attack
                {
                    Vector3 relativePoint = transform.InverseTransformPoint(player.position);
                    if (relativePoint.x > -rangedAttackAngle && relativePoint.x < rangedAttackAngle && relativePoint.z > 0)
                    {
                        attacking = true;
                        ranged = true;
                        timer = Time.time + rangedAttackDelay;
                    }
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relLoc, Vector3.up), turnSpeed * Time.deltaTime);
            }
            else if (Time.time > timer)
            {
                if (ranged)
                    RangedAttack();
                else
                    Attack();
                attacking = false;
                ranged = false;
                timer = Time.time + 1.5f;
            }
            else
            {
                relLoc.y = 0;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relLoc, Vector3.up), attackingTurnSpeed * Time.deltaTime);
            }
        }
    }

    //public override void Death()
    //{
    //    Instantiate(grubExplosion, transform.position, transform.rotation);
    //    base.Death();
    //}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
            canMove = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
            canMove = false;
    }

    void Attack()
    {
        int layerMask = 1 << 3;
        Collider[] hitColliders = Physics.OverlapBox(damageBox.position, damageBox.localScale / 2, Quaternion.identity, layerMask);

        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Player")
                hitColliders[i].transform.gameObject.GetComponent<PlayerHealth>().ProjectileHit(200, transform.forward, 10000f);
            i++;
        }
    }

    void RangedAttack()
    {
        Transform b = Instantiate(boulder, boulderSpawn.position, Quaternion.identity);
        b.GetComponent<Rigidbody>().AddForce((player.position - transform.position).normalized * rangedAttackForce + Vector3.up * rangedAttackUpForce, ForceMode.Impulse);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        Gizmos.DrawWireCube(damageBox.position, damageBox.localScale / 2);
    }
}
