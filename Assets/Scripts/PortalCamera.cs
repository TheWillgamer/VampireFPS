using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    Transform Camera;
    [SerializeField] Transform portal1;
    [SerializeField] Transform portal2;

    // Update is called once per frame
    void Update()
    {
        if (Camera == null)
        {
            Camera = GameObject.FindWithTag("MainCamera").transform;
        }
        else
        {
            Vector3 offset = Camera.position - portal1.position;
            transform.position = portal2.position + offset;

            transform.rotation = Camera.rotation;
        }

    }
}
