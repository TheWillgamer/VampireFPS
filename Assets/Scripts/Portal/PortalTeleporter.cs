using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField] Transform receiver;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Transform player = col.transform;
            if (Vector3.Dot(transform.up, player.position - transform.position) > 0f)
                player.position = receiver.position + player.position - transform.position;
        }
    }
}
