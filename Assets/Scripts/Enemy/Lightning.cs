using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private float exploTime;       // how long before lightning explosion
    [SerializeField] private float exploDelay;      // how long after lightning explosion before damage takes place
    [SerializeField] private float exploHeight;     // dictates angle of player knockback
    [SerializeField] Transform explosion;

    [SerializeField] private float damageRadius = 1;     // size of lighting explosion
    [SerializeField] private int damage = 5;
    [SerializeField] private float knockback = 5;
    [SerializeField] private int playerDamage = 5;
    [SerializeField] private float playerKnockback = 5;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explosion", exploTime);
    }

    void Explosion()
    {
        Instantiate(explosion, transform.position + new Vector3(0f, 24f, 0f), transform.rotation);
        Invoke("ExploDamage", exploDelay);
    }

    void ExploDamage()
    {
        int layermask = 1 << 10;
        layermask = ~layermask;

        foreach (Collider col in Physics.OverlapSphere(transform.position, damageRadius, layermask))
        {
            if (col.tag == "Enemy")
            {
                EnemyHitDetection ehd = col.gameObject.GetComponent<EnemyHitDetection>();
                ehd.TakeDamage(damage);
                ehd.Knockback(knockback, (col.transform.position - (transform.position + new Vector3(0f, exploHeight, 0f))).normalized);
            }
            else if (col.tag == "Player")
            {
                // Calculate Angle Between the collision point and the player
                Vector3 dir = col.transform.position - (transform.position + new Vector3(0f, exploHeight, 0f));
                // And finally we add force in the direction of dir and multiply it by force.
                //  This will push back the player
                //col.gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * playerKnockback);
                col.gameObject.GetComponent<PlayerHealth>().ProjectileHit(playerDamage, dir.normalized, playerKnockback);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        Gizmos.DrawSphere(transform.position, damageRadius);
    }
}
