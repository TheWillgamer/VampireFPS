using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Ability
{
    [SerializeField] Transform swipeEffect;
    [SerializeField] Transform swipePoint;      // where the blast effect spawns
    [SerializeField] Transform damageBox;      // where the blast effect spawns

    public int damage = 5;
    public float knockback = 10000f;
    public AudioSource swipeSound;
    private HitSlowMo hsm;

    void Start()
    {
        hsm = GetComponent<HitSlowMo>();
    }

    public override void UseAbility()
    {
        Transform swipe = Instantiate(swipeEffect, swipePoint.position, swipePoint.rotation);
        swipe.SetParent(swipePoint);

        swipeSound.Play(0);
        Collider[] hitColliders = Physics.OverlapBox(damageBox.position, damageBox.localScale / 2, Quaternion.identity);

        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Enemy")
            {
                Collider col = hitColliders[i];
                Vector3 direction = col.transform.position - transform.position;
                float distance = direction.magnitude;

                EnemyHitDetection ehd = col.gameObject.GetComponent<EnemyHitDetection>();
                ehd.TakeDamage(damage);
                ehd.Knockback(knockback, direction.normalized);
                hsm.EnableSlowMo();
            }
            i++;
        }
    }

    void Activate()
    {
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        Gizmos.DrawWireCube(damageBox.position, damageBox.localScale / 2);
    }
}
