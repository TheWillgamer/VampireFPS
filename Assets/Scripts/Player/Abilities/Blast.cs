using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : Ability
{
    public float movementReductionMultiplier = 0.2f;
    public float dashSpeed = 2000f;
    public float activateTime = 0.1f;

    [SerializeField] Transform blastEffect;
    [SerializeField] Transform blastPoint;      // where the blast effect spawns
    [SerializeField] private float radius;
    [SerializeField] private float range;

    public int minDamage = 5;
    public int maxDamage = 5;

    public float minKnockback = 10000f;
    public float maxKnockback = 10000f;

    public AudioSource sound;

    public override void UseAbility()
    {
        //sound.Play(0);

        Invoke(nameof(Activate), activateTime);
    }

    void Activate()
    {
        Instantiate(blastEffect, blastPoint.position, blastPoint.rotation);

        rb.velocity = rb.velocity * movementReductionMultiplier;
        rb.AddForce(-pm.playerCam.transform.forward * dashSpeed);

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
                ehd.TakeDamage(maxDamage - (int)((distance / range) * (maxDamage - minDamage)));
                ehd.Knockback(maxKnockback - (distance / range) * (maxKnockback - minKnockback), direction.normalized);
            }
        }
    }
}
