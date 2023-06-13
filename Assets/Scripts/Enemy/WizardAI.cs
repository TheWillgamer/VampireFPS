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
    private bool coolingDown;
    private float charge;

    public AudioSource fire;
    public AudioSource death;

    // Start is called before the first frame update
    void Start()
    {
        charge = 0;
        coolingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        //active = true;
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (!active || player == null)
            return;

        Vector3 relLoc = player.transform.GetChild(1).position - transform.position;
        relLoc.y = 0;

        if (relLoc.magnitude < alertRadius)
        {
            transform.rotation = Quaternion.LookRotation(relLoc, Vector3.up);

            if (!coolingDown)
            {
                RaycastHit hit;
                if (Physics.Raycast(rangedSpawn.position, player.transform.GetChild(1).position - rangedSpawn.position, out hit, attackRange))
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
        Instantiate(proj, rangedSpawn.position, Quaternion.LookRotation(player.transform.GetChild(1).position - rangedSpawn.position, Vector3.up));
        fire.Play();
        coolingDown = true;
    }

    public override void Death()
    {
        Instantiate(deathParticles, deathParticlesSpawn.position, Quaternion.identity);
        Destroy(transform.GetChild(0).gameObject);
        active = false;
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = false;
        }
        death.Play();

        base.Death();
    }
}
