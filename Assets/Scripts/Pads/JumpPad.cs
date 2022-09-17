using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float force;
    private bool disabled = false;


    void OnTriggerEnter(Collider other)
    {
        if (!disabled && other.tag == "Player")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 vel = rb.velocity;
            rb.velocity = new Vector3(vel.x, 0, vel.z);
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            disabled = true;
            Invoke("CancelDisabled", .2f);
        }
    }

    void CancelDisabled()
    {
        disabled = false;
    }
}
