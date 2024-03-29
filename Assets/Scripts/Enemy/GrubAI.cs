using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrubAI : EnemyAI
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float alertRadius;     // how fast enemy turns towards the player
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRange;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackKnockback;
    private bool falling;
    private Rigidbody rb;
    private float extraGravity = 2000;
    private float timer;            // for attack to initiate
    private bool attacking;         // attack has started

    [SerializeField] private GameObject grubExplosion;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        active = true;
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (!active || player == null)
            return;

        RaycastHit hit;
        int layerMask = 1 << 3;

        rb.AddForce(Vector3.down * Time.deltaTime * extraGravity);      // Extra Gravity
        Vector3 relLoc = player.transform.position - transform.position;
        anim.SetBool("moving", false);

        if (relLoc.magnitude < alertRadius && !falling)
        {
            if (!attacking)
            {
                anim.SetBool("moving", true);
                if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, layerMask))
                {
                    attacking = true;
                    anim.SetTrigger("attacking");
                    timer = Time.time + attackDelay;
                }

                // Movement
                relLoc.y = 0;
                transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relLoc, Vector3.up), turnSpeed * Time.deltaTime);
            }
            else if (Time.time > timer)
            {
                // Attack
                if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, layerMask))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        hit.transform.gameObject.GetComponent<PlayerHealth>().ProjectileHit(attackDamage, transform.forward, attackKnockback);
                    }
                }
                attacking = false;
            }
        }

        if (Physics.SphereCast(transform.position, .2f, -Vector3.up, out hit, .8f))
        {
            falling = false;
        }
        else
        {
            falling = true;
        }
    }

    public override void Death()
    {
        Instantiate(grubExplosion, transform.position, transform.rotation);
        base.Death();
    }
}
