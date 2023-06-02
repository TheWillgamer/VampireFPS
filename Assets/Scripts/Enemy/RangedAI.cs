using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI : EnemyAI
{
    public Transform cbow;

    public float horRotSpeed;      // how fast enemies rotate towards the player horizontally
    public float verRotSpeed;      // how fast the crossbow rotates towards the player vertically
    public bool dead;
    public GameObject deathParticles;

    public float fireFrequency = 1f;
    private float offcd;
    public float maxRange = 30f;
    [SerializeField] Transform proj;
    [SerializeField] Transform rangedSpawn;

    public AudioSource tense;
    public AudioSource fire;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        offcd = Time.time + fireFrequency;
        dead = false;
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        active = true;
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (!active || player == null)
            return;

        if (!dead)
        {
            // rotates enemy and crossbow towards player
            var newRotation1 = Quaternion.LookRotation(player.transform.position - transform.position);
            newRotation1.x = 0;
            newRotation1.z = 0;

            var newRotation2 = Quaternion.LookRotation(player.transform.position - cbow.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation1, Time.deltaTime * horRotSpeed);
            cbow.rotation = Quaternion.Slerp(cbow.rotation, newRotation2, Time.deltaTime * verRotSpeed);

            // fires arrow
            if(offcd < Time.time && Vector3.Distance(transform.position, player.transform.position) < maxRange)
            {
                Invoke(nameof(FireArrow), Time.deltaTime * 3f);
                tense.Play();
                offcd = Time.time + fireFrequency;
            }
        }
    }

    private void FireArrow()
    {
        Instantiate(proj, rangedSpawn.position, rangedSpawn.rotation);
        fire.Play();
    }

    public override void Death()
    {
        dead = true;
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        rb.constraints = RigidbodyConstraints.None;
        rb.mass = 10f;
        Destroy(transform.GetChild(1).gameObject);
    }
}
