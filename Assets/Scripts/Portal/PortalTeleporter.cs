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
            player.position = receiver.position + player.position - transform.position;
        }
    }
}
