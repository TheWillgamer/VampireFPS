using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAI : EnemyAI
{
    [SerializeField] private float alertRadius;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackChargeTime;
    [SerializeField] private float attackCooldownTime;
    
    [SerializeField] private Transform proj;
    [SerializeField] private Transform rangedSpawn;
    private Transform player;
    private bool active;
    private bool coolingDown;
    private float charge;

    public AudioSource fire;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        active = true;
        charge = 0;
        coolingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relLoc = player.position - transform.position;

        if (active && !coolingDown && relLoc.magnitude < alertRadius)
        {
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                if (hit.collider.tag == "Player")
                {
                    charge += Time.deltaTime;
                    if (charge > attackChargeTime)
                        Fire();
                }
                else
                {
                    charge = 0;
                }
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
    void Fire()
    {
        charge = 0;
        Instantiate(proj, rangedSpawn.position, rangedSpawn.rotation);
        fire.Play();
        coolingDown = true;
    }
}
