using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : EnemyAI
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float alertRadius;     // how fast enemy turns towards the player
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackKnockback;
    [SerializeField] private float attackSelfKnockback;     // how far enemy gets knocked back after dealing damage

    [SerializeField] private GameObject ghostExplosion;

    // Update is called once per frame
    void Update()
    {
        active = true;
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
        if (!active || player == null)
            return;

        RaycastHit hit;
        int layerMask = 1 << 3;

        Vector3 relLoc = player.position - transform.position;

        if (relLoc.magnitude < alertRadius)
        {
            // Movement
            transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relLoc, Vector3.up), turnSpeed * Time.deltaTime);
        }
    }

    public override void Death()
    {
        //Instantiate(ghostExplosion, transform.position, transform.rotation);
        base.Death();
    }
}
