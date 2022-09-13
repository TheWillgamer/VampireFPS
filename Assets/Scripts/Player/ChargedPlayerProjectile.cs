using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedPlayerProjectile : MonoBehaviour
{
    [SerializeField] float aliveTime = 5.0f;

    public int minDamage = 5;
    public int maxDamage = 5;

    public float minSpeed = 30.0f;
    public float maxSpeed = 30.0f;

    public float minKnockback = 10000f;
    public float maxKnockback = 10000f;

    public float playerMinKnockback = 300f;
    public float playerMaxKnockback = 300f;

    float deathTime;
    private Rigidbody rb;
    private float _colliderRadius;
    private float radius;
    private bool active;

    public float extraGravity = 3000f;
    public float chargeValue;

    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + aliveTime;
        SphereCollider sc = GetComponent<SphereCollider>();
        _colliderRadius = sc.radius;

        radius = _colliderRadius + 10f;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTime)
        {
            Destroy(transform.parent.gameObject);
        }

        if (active)
        {
            MoveProjectile();
        }
    }

    void MoveProjectile()
    {
        //Determine how far object should travel this frame.
        float travelDistance = (minSpeed * Time.deltaTime);
        //Set trace distance to be travel distance + collider radius.
        float traceDistance = travelDistance + _colliderRadius;

        //Explode bullet if it goes through the wall
        RaycastHit hit;
        // Does the ray intersect any walls

        if (Physics.SphereCast(transform.position, _colliderRadius, transform.TransformDirection(Vector3.forward), out hit, traceDistance))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                transform.position = hit.point;
                Explode();
                Invoke("DestroyProjectile", 1.5f);
                active = false;
            }
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, traceDistance))
        {
            if (hit.transform.gameObject.tag != "Enemy")
            {
                transform.position = hit.point;
                Explode();
                Invoke("DestroyProjectile", 1.5f);
                active = false;
            }
        }

        transform.position += transform.forward * travelDistance;
    }

    void DestroyProjectile()
    {
        Destroy(transform.parent.gameObject);
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
            else if(col.tag == "Player")
            {
                // Calculate Angle Between the collision point and the player
                Vector3 dir = col.transform.position - transform.position;
                // And finally we add force in the direction of dir and multiply it by force.
                // This will push back the player
                col.gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * (playerMinKnockback + chargeValue * (playerMaxKnockback - playerMinKnockback)));
            }
        }
        Destroy(transform.parent.gameObject);
    }
}
