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
    [SerializeField] private float rangedAttackDelay;   // How long before the head fires out
    [SerializeField] private float rangedAttackCD;      // How long before it can fire again
    [SerializeField] private float rangedAttackRange;
    [SerializeField] private float rangedAttackForce;
    [SerializeField] private float rangedAttackUpForce;
    [SerializeField] private float rangedAttackAngle;
    [SerializeField] private Transform damageBox;
    [SerializeField] private GolemHead head;
    [SerializeField] private Transform headPrefab;
    [SerializeField] private Transform headPrefabSpawn;
    [SerializeField] private float headRegenTimeDelay;
    [SerializeField] private Transform headTracker;
    [SerializeField] private Transform pullBackTracker;     // Where the head is pulled back before the throw

    private bool falling;
    private Rigidbody rb;
    private float extraGravity = 2000;
    private float timer;            // for attack to initiate
    private bool attacking;         // attack has started
    private bool ranged;         // attack has started
    private bool regening;         // head is currently regenning

    private bool canMove;           // there is space in front of the enemy to move
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        canMove = true;
        ranged = false;
        regening = false;
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
        anim.SetBool("Walking", false);

        if(head == null && !regening)
        {
            regening = true;
            timer = Time.time + headRegenTimeDelay;
        }

        if (relLoc.magnitude < alertRadius && !regening)
        {
            if (!attacking)
            {
                if (relLoc.magnitude < attackRange && Time.time > timer)
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
                    Vector3 relativePoint = transform.InverseTransformPoint(player.transform.position);
                    if (Time.time > timer && relativePoint.x > -rangedAttackAngle && relativePoint.x < rangedAttackAngle && relativePoint.z > 0 && head != null && head.active)
                    {
                        anim.SetTrigger("Throw");
                        head.thrown = true;
                        head.timer = 0f;
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
                timer = Time.time + 1f;
            }
            else
            {
                relLoc.y = 0;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relLoc, Vector3.up), attackingTurnSpeed * Time.deltaTime);
            }
        }
        else if (regening)      // Regenerate the golem head
        {
            if (Time.time > timer)
            {
                Transform temp = Instantiate(headPrefab, headPrefabSpawn.position, headPrefabSpawn.rotation);
                temp.parent = transform;
                head = temp.GetComponent<GolemHead>();
                head.headTracker = headTracker;
                head.pullBackTracker = pullBackTracker;
                timer = Time.time + rangedAttackCD;
                regening = false;
            }
        }
        else if (active && ranged && Time.time > timer)
        {
            RangedAttack();
            attacking = false;
            ranged = false;
        }
    }

    public override void Death()
    {
        if (head != null)
        {
            head.host = false;
            head.thrown = false;
        }
        
        base.Death();
    }

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
        head.Throw(((player.transform.position - transform.position).normalized + Vector3.up * (player.transform.position - transform.position).magnitude / 40f).normalized);
        head = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        Gizmos.DrawWireCube(damageBox.position, damageBox.localScale / 2);
    }
}
