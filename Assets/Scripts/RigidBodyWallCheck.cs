using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyWallCheck : MonoBehaviour
{
    public Transform playerTop;
    public Transform playerBot;
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
        float traceDistance = travelDistance + _colliderRadius * 2;

        // only checks for walls
        int layerMask = 1 << 6;
        RaycastHit hit;

        // Does the ray intersect any walls
        if (velocity.magnitude > 5f && Physics.SphereCast(playerTop.position, _colliderRadius, velocity, out hit, traceDistance, layerMask))
        {
            if(velocity.magnitude > 30f && hit.collider.tag == "Destructible")
            {
                rb.velocity = 2 * rb.velocity / 3;
                hit.collider.gameObject.GetComponent<DestructibleWall>().Destroy();
            }
                rb.velocity = rb.velocity.normalized;
        }
        if (velocity.y < -45f && Physics.SphereCast(playerBot.position, _colliderRadius, velocity, out hit, traceDistance, layerMask))
        {
            if (hit.collider.tag == "Target")
                hit.collider.gameObject.GetComponent<SlammableButton>().Slam();
            GetComponent<PlayerMovement>().HardLanding();
            rb.velocity = 3 * rb.velocity / 4;
        }
    }
}
