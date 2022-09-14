using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] float speed = 30.0f;
    [SerializeField] float aliveTime = 5.0f;
    public int damage = 5;
    float deathTime;
    private float _colliderRadius;
    private bool active;
    public GameObject deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + aliveTime;
        SphereCollider sc = GetComponent<SphereCollider>();
        _colliderRadius = sc.radius;
        active = true;

        MoveProjectile();
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
        float travelDistance = (speed * Time.deltaTime);
        //Set trace distance to be travel distance + collider radius.
        float traceDistance = travelDistance + _colliderRadius;

        //Explode bullet if it goes through the wall
        RaycastHit hit;
        // Does the ray intersect any walls

        if (Physics.SphereCast(transform.position, _colliderRadius, transform.TransformDirection(Vector3.forward), out hit, traceDistance))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<EnemyHitDetection>().TakeDamage(damage);
                Debug.Log(hit.normal);
                Instantiate(deathParticles, hit.point, Quaternion.LookRotation(hit.normal));
                Invoke("DestroyProjectile", 1.5f);
                active = false;
            }
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, traceDistance))
        {
            if (hit.transform.gameObject.tag != "Enemy")
            {
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
}
