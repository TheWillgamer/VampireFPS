using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class ParticleCollector : MonoBehaviour
{
    ParticleSystem ps;
    GameObject player;
    public int healAmt = 1;     // how much each particle heals

    List <ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        ps = GetComponent<ParticleSystem>();
        ps.trigger.AddCollider(player.GetComponent<CapsuleCollider>());
    }

    private void OnParticleTrigger()
    {
        int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            player.GetComponent<PlayerHealth>().TakeDamage(-healAmt);
            particles[i] = p;

        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
    }
}
