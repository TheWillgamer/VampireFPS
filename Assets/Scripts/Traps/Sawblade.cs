using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float cd;
    [SerializeField] private float knockback;
    private float offcd;
    private PlayerHealth ph;
    private bool disabled;

    void Start()
    {
        ph = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        offcd = Time.time;
        disabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (!disabled && Time.time > offcd && other.tag == "Player")
        {
            ph.TakeDamage(damage);
            ph.Knockback(knockback, (other.transform.position - transform.position).normalized);
            offcd = Time.time + cd;
        }
    }
}
