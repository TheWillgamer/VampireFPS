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
    [SerializeField] private float directionModifier;
    public int enemyKillRecharge;

    public int minDamage = 5;
    public int maxDamage = 5;

    public float minKnockback = 10000f;
    public float maxKnockback = 10000f;

    public AudioSource chargeSound;
    public AudioSource shootSound;

    public override void UseAbility()
    {
        //chargeSound.Play(0);

        Invoke(nameof(Activate), activateTime);
    }

    void Activate()
    {
        Instantiate(blastEffect, blastPoint.position, blastPoint.rotation);
        shootSound.Play(0);

        Vector3 blastA = Vector3.Project(pm.playerCam.transform.forward, rb.velocity);      // Parallel direction
        Vector3 blastE = pm.playerCam.transform.forward - blastA;
        float modifier = 1f + rb.velocity.magnitude / directionModifier;

        if (rb.velocity.magnitude > 1f)
        {
            if (Vector3.Dot(rb.velocity, blastA) > 0)
                rb.AddForce(-blastE * dashSpeed - blastA * dashSpeed * modifier);       // blast in direction of movement
            else
                rb.AddForce(-blastE * dashSpeed - blastA * dashSpeed / modifier);

        }
        else
            rb.AddForce(-pm.playerCam.transform.forward * dashSpeed);       // player is still

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
