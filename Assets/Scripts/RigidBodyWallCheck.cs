using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyWallCheck : MonoBehaviour
{
    public Transform playerCam;
    private Rigidbody rb;
    private float _colliderRadius;

    // Start is called before the first frame update
    void Start()
    {
        CapsuleCollider cc = GetComponent<CapsuleCollider>();
        _colliderRadius = cc.radius;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = rb.velocity;
        //Determine how far object should travel this frame.
        float travelDistance = (velocity.magnitude * Time.deltaTime);
        //Set trace distance to be travel distance + collider radius.
        float traceDistance = travelDistance + _colliderRadius;

        // only checks for walls
        int layerMask = 1 << 6;

        //Explode bullet if it goes through the wall
        RaycastHit hit;
        // Does the ray intersect any walls

        if (velocity.magnitude > 30f && Physics.SphereCast(playerCam.position, _colliderRadius, velocity, out hit, traceDistance, layerMask))
        {
            rb.velocity = rb.velocity / 5;
            Debug.Log("HI");
        }
    }
}
