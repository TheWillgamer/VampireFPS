using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitRagdoll : MonoBehaviour
{
    [SerializeField] float speed = 30.0f;
    [SerializeField] float aliveTime = 5.0f;
    [SerializeField] float spinSpeed = 5.0f;
    float deathTime;
    private bool active;
    public GameObject deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + aliveTime;
        active = true;

        MoveProjectile();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTime)
        {
            Instantiate(deathParticles, transform.position, transform.rotation);
            Destroy(gameObject);
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

        //Explode bullet if it goes through the wall
        RaycastHit hit;

        // Does the ray intersect any walls
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, travelDistance))
        {
            Instantiate(deathParticles, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(gameObject);
        }

        transform.position -= transform.up * travelDistance;
        transform.GetChild(0).Rotate(0f, 0f, -spinSpeed * Time.deltaTime, Space.Self);
    }
}
