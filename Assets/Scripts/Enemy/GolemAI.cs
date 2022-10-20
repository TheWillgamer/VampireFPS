using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAI : EnemyAI
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float alertRadius;     // how fast enemy turns towards the player
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRange;
    [SerializeField] private float rangedAttackForce;
    [SerializeField] private float rangedAttackUpForce;
    [SerializeField] private Transform boulder;
    [SerializeField] private Transform boulderSpawn;
    private Transform player;
    private bool active;
    private bool falling;
    private Rigidbody rb;
    private float extraGravity = 2000;
    private float timer;            // for attack to initiate
    private bool attacking;         // attack has started

    private bool canMove;           // there is space in front of the enemy to move


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        active = true;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        int layerMask = 1 << 3;

        rb.AddForce(Vector3.down * Time.deltaTime * extraGravity);      // Extra Gravity
        Vector3 relLoc = player.position - transform.position;

        if (active && relLoc.magnitude < alertRadius)
        {
            if (!attacking)
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, layerMask))
                {
                    attacking = true;
                    timer = Time.time + attackDelay;
                }

                // Movement
                relLoc.y = 0;
                if (canMove)
                    transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relLoc, Vector3.up), turnSpeed * Time.deltaTime);
            }
            else if (Time.time > timer)
            {
                RangedAttack();
                attacking = false;
            }
        }
    }

    //public override void Death()
    //{
    //    Instantiate(grubExplosion, transform.position, transform.rotation);
    //    base.Death();
    //}

    void OnTriggerEnter(Collider other)
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
        RaycastHit hit;
        int layerMask = 1 << 3;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, layerMask))
        {
            hit.transform.gameObject.GetComponent<PlayerHealth>().ProjectileHit(200, transform.forward, 10000f);
        }
    }

    void RangedAttack()
    {
        Transform b = Instantiate(boulder, boulderSpawn.position, Quaternion.identity);
        b.GetComponent<Rigidbody>().AddForce((player.position - transform.position).normalized * rangedAttackForce + Vector3.up * rangedAttackUpForce, ForceMode.Impulse);
    }
}
