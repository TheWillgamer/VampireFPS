using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAI : EnemyAI
{
    [SerializeField] private float alertRadius;
    [SerializeField] private float attackCooldownTime;
    [SerializeField] private Transform lightning;
    private Transform player;
    private bool coolingDown;
    private float charge;
    private bool active;
    private bool attacking;

    public AudioSource l_sound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        active = true;
        charge = 0;
        coolingDown = false;
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relLoc = player.position - transform.position;

        if (active && !coolingDown && relLoc.magnitude < alertRadius)
        {
            if (charge > 1)
            {
                int layerMask = 1 << 6;
                RaycastHit hit;

                if (Physics.Raycast(player.position, Vector3.down, out hit, 20, layerMask))
                {
                    Attack(hit.point);
                    coolingDown = true;
                }
                charge = 0;
            }
            else
            {
                charge += Time.deltaTime;
            }
        }
        else if (coolingDown)
        {
            charge += Time.deltaTime;
            if (charge > attackCooldownTime)
            {
                charge = 0;
                coolingDown = false;
            }
        }
    }
    void Attack(Vector3 loc)
    {
        charge = 0;
        Instantiate(lightning, loc, Quaternion.identity);
        l_sound.Play();
        coolingDown = true;
    }
}
