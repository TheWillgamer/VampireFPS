using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_raycast : MonoBehaviour
{
    [SerializeField] float speed = 30.0f;
    [SerializeField] float aliveTime = 5.0f;
    [SerializeField] float knockback = 1000f;
    [SerializeField] Transform hitExplosion;
    private bool active;
    public int damage = 5;
    float deathTime;

    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + aliveTime;

        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTime)
        {
            Destroy(gameObject);
        }

        if (active)
            MoveProjectile();
    }

    void MoveProjectile()
    {
        //Determine how far object should travel this frame.
        float travelDistance = (speed * Time.deltaTime);

        //Explode bullet if it goes through the wall
        RaycastHit hit;
        // Does the ray intersect any walls

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, travelDistance))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                hit.transform.gameObject.GetComponent<PlayerHealth>().ProjectileHit(damage, transform.forward, knockback);
                //Invoke("DestroyProjectile", 1.5f);
                //active = false;
            }
            Instantiate(hitExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        transform.position += transform.forward * travelDistance;
    }
}