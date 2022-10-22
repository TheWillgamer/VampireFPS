using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Ability
{
    public float movementReductionMultiplier = 0.2f;
    public float dashSpeed = 2000f;
    public float activateTime = 0.1f;

    [SerializeField] Transform swipeEffect;
    [SerializeField] Transform swipePoint;      // where the blast effect spawns
    [SerializeField] private float radius;
    [SerializeField] private float range;

    public int damage = 5;
    public float knockback = 10000f;
    public AudioSource swipeSound;

    public override void UseAbility()
    {
        //chargeSound.Play(0);

        Invoke(nameof(Activate), activateTime);
    }

    void Activate()
    {
        Instantiate(swipeEffect, swipePoint.position, swipePoint.rotation);
        //swipeSound.Play(0);

        //RaycastHit[] hits;
        //hits = Physics.SphereCastAll(pm.playerCam.transform.position, radius, pm.playerCam.transform.forward, range);

        //for (int i = 0; i < hits.Length; i++)
        //{
        //    Collider col = hits[i].collider;

        //    if (col.tag == "Enemy")
        //    {
        //        Vector3 direction = col.transform.position - transform.position;
        //        float distance = direction.magnitude;

        //        EnemyHitDetection ehd = col.gameObject.GetComponent<EnemyHitDetection>();
        //        ehd.TakeDamage(maxDamage - (int)((distance / range) * (maxDamage - minDamage)));
        //        ehd.Knockback(maxKnockback - (distance / range) * (maxKnockback - minKnockback), direction.normalized);
        //    }
        //}
    }
}
