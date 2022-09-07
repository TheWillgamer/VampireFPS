using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : Ability
{
    public float dashSpeed = 2000f;
    public float activateTime = 0.1f;

    [SerializeField] Transform blastPoint;
    [SerializeField] private float radius;
    private float reach;

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
        rb.AddForce(-pm.playerCam.transform.forward * dashSpeed);
        reach = (blastPoint.position - transform.position).magnitude;

        // blasts enemies away
        foreach (Collider col in Physics.OverlapCapsule(transform.position, blastPoint.position, radius))
        {
            if (col.tag == "Enemy")
            {
                Vector3 direction = col.transform.position - transform.position;
                float distance = direction.magnitude;

                EnemyHitDetection ehd = col.gameObject.GetComponent<EnemyHitDetection>();
                ehd.TakeDamage(maxDamage - (int)((distance / reach) * (maxDamage - minDamage)));
                ehd.Knockback(maxKnockback - (distance / reach) * (maxKnockback - minKnockback), direction.normalized);
            }
        }
    }
}
