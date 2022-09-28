using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlayerToPlatform : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "Player")
        {
            other.collider.transform.SetParent(transform);
        }
    }
    void OnCollisionExit(Collision other)
    {
        other.collider.transform.SetParent(null);
    }
}
