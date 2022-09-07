using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedPlayerProjectile : MonoBehaviour
{
    [SerializeField] float force = 30.0f;
    [SerializeField] float aliveTime = 5.0f;

    public int minDamage = 5;
    public int maxDamage = 5;

    public float minKnockback = 10000f;
    public float maxKnockback = 10000f;

    float deathTime;
    private Rigidbody rb;
    private float _colliderRadius;
    private float radius;

    public float extraGravity = 3000f;
    public float chargeValue;

    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + aliveTime;
        SphereCollider sc = GetComponent<SphereCollider>();
        _colliderRadius = sc.radius;

        radius = _colliderRadius + 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Ground")
        {
            Explode();
        }
    }

    public void Launch()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    private void Explode()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, radius))
        {
            if (col.tag == "Enemy")
            {
                EnemyHitDetection ehd = col.gameObject.GetComponent<EnemyHitDetection>();
                ehd.TakeDamage(minDamage + (int)(chargeValue * (maxDamage - minDamage)));
                ehd.Knockback(minKnockback + chargeValue * (maxKnockback - minKnockback), (col.transform.position - transform.position).normalized);
            }
        }
        Destroy(transform.parent.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * extraGravity);

        if (Time.time > deathTime)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
