using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAI : EnemyAI
{
    [SerializeField] private float alertRadius;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackChargeTime;
    [SerializeField] private Transform laserStart;
    private Transform player;
    private bool active;
    private float charge;
    LineRenderer lr;
    Vector3[] points;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        active = true;
        lr = GetComponent<LineRenderer>();
        points = new Vector3[2];
        charge = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relLoc = player.position - transform.position;

        if (active && relLoc.magnitude < alertRadius)
        {
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                points[0] = laserStart.position;
                if (hit.collider.tag == "Player")
                {
                    charge += Time.deltaTime;
                    if (charge > attackChargeTime)
                        Fire();
                    lr.endWidth = 1f - charge / attackChargeTime;
                    points[1] = player.transform.position - transform.forward * .4f;
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
                points[0] = transform.position;
                points[1] = transform.position + transform.forward * attackRange;
                lr.SetPositions(points);
            }
        }
    }
    void Fire()
    {
        charge = 0;
    }
}
