using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Ability
{
    public float dashSpeed = 2000f;

    [SerializeField] private float radius;
    [SerializeField] private float maxRange;
    private float reach;

    public int damage = 5;
    public float knockback = 10000f;

    public AudioSource sound;

    public override void UseAbility()
    {
        rb.AddForce(-pm.playerCam.transform.forward * dashSpeed);

        RaycastHit hit;
        int layerMask = 1 << 6;
        float range = maxRange;  // how far the beam goes

        if (Physics.Raycast(pm.playerCam.transform.position, pm.playerCam.transform.forward, out hit, maxRange, layerMask))
        {
            Debug.DrawRay(pm.playerCam.transform.position, pm.playerCam.transform.forward * hit.distance, Color.yellow);
            range = hit.distance;
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.Log("Did not Hit");
        }

        RaycastHit[] hits;
        hits = Physics.SphereCastAll(pm.playerCam.transform.position, radius, pm.playerCam.transform.forward, range);

        for (int i = 0; i < hits.Length; i++)
        {
            Collider col = hits[i].collider;

            if (col.tag == "Enemy")
            {
                Vector3 direction = col.transform.position - transform.position;
                float distance = direction.magnitude;

                EnemyHitDetection ehd = col.gameObject.GetComponent<EnemyHitDetection>();
                ehd.TakeDamage(damage);
                ehd.Knockback(knockback, direction.normalized);
            }
        }

        // blasts enemies away
        //foreach (Collider col in Physics.OverlapCapsule(transform.position, blastPoint.position, radius))
        //{
        //    if (col.tag == "Enemy")
        //    {
        //        Vector3 direction = col.transform.position - transform.position;
        //        float distance = direction.magnitude;

        //        EnemyHitDetection ehd = col.gameObject.GetComponent<EnemyHitDetection>();
        //        ehd.TakeDamage(maxDamage - (int)((distance / reach) * (maxDamage - minDamage)));
        //        ehd.Knockback(maxKnockback - (distance / reach) * (maxKnockback - minKnockback), direction.normalized);
        //    }
        //}
    }
}
