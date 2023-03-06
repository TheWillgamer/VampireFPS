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
    private Rigidbody rb;
    private Transform playerLoc;
    private bool disabled;

    void Start()
    {
        GetPlayer();
        offcd = Time.time;
        disabled = false;

        GameplayManager.Spawn += GetPlayer;
    }

    void GetPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        ph = player.GetComponent<PlayerHealth>();
        rb = player.GetComponent<Rigidbody>();
        playerLoc = player.transform;
    }

    void OnTriggerStay(Collider other)
    {
        if (!disabled && Time.time > offcd && other.tag == "Player")
        {
            ph.TakeDamage(damage);
            Vector3 direction = playerLoc.position - transform.position;
            direction = new Vector3(direction.x, 0, direction.z);
            rb.velocity = Vector3.zero;
            rb.AddForce(direction.normalized * knockback + transform.up * knockback / 2);
            offcd = Time.time + cd;
        }
    }
}
