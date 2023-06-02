using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : EnemyAI
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float alertRadius;     // how fast enemy turns towards the player
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackDelay;     // delay after attacking
    [SerializeField] private float attackKnockback;
    [SerializeField] private float attackSelfBackSpeed;     // how fast enemy gets knocked back after dealing damage

    [SerializeField] private GameObject ghostExplosion;

    private float timer;
    private bool alerted;           // when enemy sees player for the first time

    void Start()
    {
        timer = -20f;
        alerted = false;
    }

    // Update is called once per frame
    void Update()
    {
        active = true;
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (!active || player == null)
            return;

        Vector3 relLoc = player.transform.position - transform.position;
        if (!alerted)
        {
            transform.rotation = Quaternion.LookRotation(relLoc, Vector3.up);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, alertRadius))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    alerted = true;
                    Debug.Log("alerted!");
                }
            }
        }
        else
        {
            if (Time.time > timer)
            {
                // Movement
                transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relLoc, Vector3.up), turnSpeed * Time.deltaTime);
            }
            else if (timer > Time.time)
            {
                transform.position = transform.position + -transform.forward * attackSelfBackSpeed * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Time.time > timer)
        {
            other.gameObject.GetComponent<PlayerHealth>().ProjectileHit(attackDamage, (player.transform.position - transform.position).normalized, attackKnockback);
            timer = Time.time + attackDelay;
        }
    }

    public override void Death()
    {
        //Instantiate(ghostExplosion, transform.position, transform.rotation);
        base.Death();
    }
}
