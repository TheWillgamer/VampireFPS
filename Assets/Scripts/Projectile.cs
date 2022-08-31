using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 30.0f;
    [SerializeField] float aliveTime = 5.0f;
    float deathTime;
    private float _colliderRadius;

    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + aliveTime;
        SphereCollider sc = GetComponent<SphereCollider>();
        _colliderRadius = sc.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTime)
        {
            Destroy(transform.parent.gameObject);
        }

        //Determine how far object should travel this frame.
        float travelDistance = (speed * Time.deltaTime);
        //Set trace distance to be travel distance + collider radius.
        float traceDistance = travelDistance + _colliderRadius;

        //Explode bullet if it goes through the wall
        int layerMask = 1 << 6;

        RaycastHit hit;
        // Does the ray intersect any walls

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, traceDistance, layerMask))
        {
            Destroy(transform.parent.gameObject);
        }

        transform.position += transform.forward * travelDistance;
    }
}
