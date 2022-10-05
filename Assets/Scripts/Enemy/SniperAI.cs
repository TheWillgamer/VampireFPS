using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAI : MonoBehaviour
{
    [SerializeField] private float turnSpeed;   // how fast enemy turns towards the player
    [SerializeField] private float alertRadius;
    [SerializeField] private float attackRange;
    private Transform player;
    private bool active;
    LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        active = true;
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relLoc = player.position - transform.position;

        if (active && relLoc.magnitude < alertRadius)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relLoc, Vector3.up), turnSpeed * Time.deltaTime);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                Vector3[] points = new Vector3[2];
                points[0] = transform.position;
                points[1] = hit.point;
                lr.SetPositions(points);
            }
            else
            {
                Vector3[] points = new Vector3[2];
                points[0] = transform.position;
                points[1] = transform.position + transform.forward * attackRange;
                lr.SetPositions(points);
            }
        }
    }
}
