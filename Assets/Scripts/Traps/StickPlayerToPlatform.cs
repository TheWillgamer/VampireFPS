using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlayerToPlatform : MonoBehaviour
{
    void OnCollisionStay(Collision other)
    {
        other.gameObject.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
    }
}
