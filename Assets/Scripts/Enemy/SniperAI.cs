using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAI : EnemyAI
{
    [SerializeField] private float alertRadius;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackChargeTime;
    [SerializeField] private float attackCooldownTime;
    [SerializeField] private Transform laserStart;
    [SerializeField] private Transform proj;
    [SerializeField] private Transform bunnyLights;
    private bool coolingDown;
    private float charge;
    LineRenderer lr;
    Vector3[] points;
    private bool dead;

    public AudioSource fire;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        points = new Vector3[2];
        charge = 0;
        coolingDown = false;
        lr.endWidth = 0;
        anim = transform.GetChild(0).GetComponent<Animator>();
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        active = true;
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
        if (!active || player == null || dead)
            return;

        Vector3 relLoc = player.position - transform.position;

        bunnyLights.localRotation = Quaternion.Euler(new Vector3(Mathf.Rad2Deg * Mathf.Asin(relLoc.y / relLoc.magnitude), 0f, 0f));
        relLoc.y = 0;
        transform.rotation = Quaternion.LookRotation(relLoc, Vector3.up);

        if (!coolingDown && relLoc.magnitude < alertRadius)
        {
            RaycastHit hit;
            if (Physics.Raycast(laserStart.position, laserStart.forward, out hit, attackRange))
            {
                points[0] = laserStart.position;
                if (hit.collider.tag == "Player")
                {
                    charge += Time.deltaTime;
                    lr.endWidth = 1f - charge / attackChargeTime;
                    points[1] = player.position - laserStart.forward * .4f;
                    if (charge > attackChargeTime)
                        Fire();
                }
                else
                {
                    charge = 0;
                    points[1] = hit.point;
                }
                    
                lr.SetPositions(points);
            }
            else
            {
                points[0] = laserStart.position;
                points[1] = laserStart.position + laserStart.forward * attackRange;
                lr.SetPositions(points);
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
        Instantiate(proj, laserStart.position, laserStart.rotation);
        fire.Play();
        coolingDown = true;
        lr.endWidth = 0;
    }

    public override void Death()
    {
        anim.SetTrigger("Death");
        active = false;
        GetComponent<Collider>().enabled = false;
        dead = true;
    }
}
