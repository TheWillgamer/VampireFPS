using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongSplineRigidbody : MonoBehaviour
{
    [SerializeField] private int speed;
    
    private Transform playerLoc;
    private int next_point;     // where the sawblade is traveling to
    private Rigidbody rb;

    [SerializeField] private Transform[] points;        // points in the path of the sawblade

    void Start()
    {
        next_point = 0;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if ((transform.position - points[next_point].position).magnitude < .01f)
        {
            next_point++;
            if (next_point >= points.Length)
                next_point = 0;
        }

        rb.velocity = points[next_point].position - transform.position;

        //transform.position = Vector3.MoveTowards(transform.position, points[next_point].position, speed * Time.deltaTime);
        Debug.Log(rb.velocity);
    }
}
