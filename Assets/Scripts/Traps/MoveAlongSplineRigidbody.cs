using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongSplineRigidbody : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float wait;      // How long to wait before moving to the next point
    
    private Transform playerLoc;
    private int next_point;     // where the sawblade is traveling to
    private Rigidbody rb;
    private float offcd;
    private bool startwait;     // when platform first reaches the point

    [SerializeField] private Transform[] points;        // points in the path of the sawblade

    void Start()
    {
        next_point = 0;
        rb = GetComponent<Rigidbody>();
        rb.velocity = (points[next_point].position - transform.position).normalized * speed;
        offcd = Time.time;
        startwait = false;
    }

    void Update()
    {
        if ((transform.position - points[next_point].position).magnitude < 1f)
        {
            if (!startwait)
            {
                offcd = Time.time + wait;
                startwait = true;
            }
            if(Time.time > offcd)
            {
                next_point++;
                if (next_point >= points.Length)
                    next_point = 0;
                rb.velocity = (points[next_point].position - transform.position).normalized * speed;
                startwait = false;
            }
        }
    }
}
