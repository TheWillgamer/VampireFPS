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
    [SerializeField] private Transform deathParticlesSpawn;
    [SerializeField] private GameObject deathParticles;
    private Transform player;
    private bool active;
    private bool coolingDown;
    private float charge;

    public AudioSource fire;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform.GetChild(1);
        active = true;
        charge = 0;
        coolingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relLoc = player.position - transform.position;
        relLoc.y = 0;

        if (active && relLoc.magnitude < alertRadius)
        {
            transform.rotation = Quaternion.LookRotation(relLoc, Vector3.up);

            if (!coolingDown)
            {
                RaycastHit hit;
                if (Physics.Raycast(rangedSpawn.position, player.position - rangedSpawn.position, out hit, attackRange))
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
            else
            {
                charge += Time.deltaTime;
                if (charge > attackCooldownTime)
                {
                    charge = 0;
                    coolingDown = false;
                }
            }
        }
    }
    void Fire()
    {
        charge = 0;
        Instantiate(proj, rangedSpawn.position, Quaternion.LookRotation(player.position - rangedSpawn.position, Vector3.up));
        fire.Play();
        coolingDown = true;
    }

    public override void Death()
    {
        Instantiate(deathParticles, deathParticlesSpawn.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
