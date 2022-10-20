using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_arcing : MonoBehaviour
{
    [SerializeField] private float aliveTime = 10.0f;
    [SerializeField] private int damage = 5;
    [SerializeField] private float knockback = 5;
    float deathTime;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + aliveTime;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerHealth>().ProjectileHit(damage, rb.velocity.normalized, knockback);
        }
            
        Destroy(gameObject);
    }
}
