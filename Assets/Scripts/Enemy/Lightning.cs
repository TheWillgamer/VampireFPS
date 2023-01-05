using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private float hitTime;         // how long before damage is done
    [SerializeField] private float exploTime;       // how long before lightning explosion after player is hit by lightning
    [SerializeField] Transform explosion;

    [SerializeField] private float damageHeight = 300;   // height of lighting strike
    [SerializeField] private float damageRadius = 1;     // size of lighting explosion
    [SerializeField] private int damage = 5;
    [SerializeField] private float knockback = 5;
    [SerializeField] private int playerDamage = 5;
    [SerializeField] private float playerKnockback = 5;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Strike", hitTime);
    }

    // Hits everything in the collider
    void Strike()
    {
        int layermask = 1 << 10;
        layermask = ~layermask;

        foreach (Collider col in Physics.OverlapCapsule(transform.position, transform.position + new Vector3(0, damageHeight, 0), 1f, layermask))
        {
            if (col.tag == "Player")
            {
                // Calculate Angle Between the collision point and the player
                Vector3 dir = col.transform.position - transform.position;
                // And finally we add force in the direction of dir and multiply it by force.
                //  This will push back the player
                //col.gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * playerKnockback);
                col.gameObject.GetComponent<PlayerHealth>().ProjectileHit(playerDamage, Vector3.down, playerKnockback);
            }
        }

        Invoke("Explosion", exploTime);
    }

    void Explosion()
    {
        int layermask = 1 << 10;
        layermask = ~layermask;

        foreach (Collider col in Physics.OverlapSphere(transform.position, damageRadius, layermask))
        {
            if (col.tag == "Enemy")
            {
                EnemyHitDetection ehd = col.gameObject.GetComponent<EnemyHitDetection>();
                ehd.TakeDamage(damage);
                ehd.Knockback(knockback, (col.transform.position - transform.position).normalized);
            }
            else if (col.tag == "Player")
            {
                // Calculate Angle Between the collision point and the player
                Vector3 dir = col.transform.position - transform.position;
                // And finally we add force in the direction of dir and multiply it by force.
                //  This will push back the player
                //col.gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * playerKnockback);
                col.gameObject.GetComponent<PlayerHealth>().ProjectileHit(playerDamage, dir.normalized, playerKnockback);
            }
        }

        Instantiate(explosion, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
