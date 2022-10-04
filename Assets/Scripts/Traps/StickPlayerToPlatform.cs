using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlayerToPlatform : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
            pm.prb = GetComponent<Rigidbody>();
            pm.onMovingPlatform = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
            pm.onMovingPlatform = false;
        }
    }
}
