using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int damage;
    [SerializeField] private float cd;
    [SerializeField] private float knockback;
    private float offcd;
    private PlayerHealth ph;
    private Rigidbody rb;
    private Transform playerLoc;
    private bool disabled;
    private int next_point;     // where the sawblade is traveling to

    [SerializeField] private Transform[] points;        // points in the path of the sawblade

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        ph = player.GetComponent<PlayerHealth>();
        rb = player.GetComponent<Rigidbody>();
        playerLoc = player.transform;
        offcd = Time.time;
        disabled = false;
        next_point = 0;
    }

    void OnTriggerStay(Collider other)
    {
        if (!disabled && Time.time > offcd && other.tag == "Player")
        {
            ph.TakeDamage(damage);
            Vector3 direction = playerLoc.position - transform.position;
            direction = new Vector3(direction.x, 0, direction.z);
            rb.velocity = Vector3.zero;
            rb.AddForce(direction.normalized * knockback + transform.up * knockback / 2);
            offcd = Time.time + cd;
        }
    }

    void Update()
    {
        if(transform.position == points[next_point].position)
        {
            next_point++;
            if (next_point >= points.Length)
                next_point = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, points[next_point].position, speed * Time.deltaTime);
    }
}
