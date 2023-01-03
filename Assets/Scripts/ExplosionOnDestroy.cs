using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionOnDestroy : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float damageRadius = 5;
    [SerializeField] private float knockback = 5;
    [SerializeField] private int playerDamage = 5;
    [SerializeField] private float playerKnockback = 5;
    [SerializeField] Transform explosion;

    void OnDestroy()
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
    }
}
