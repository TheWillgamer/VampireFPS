using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    Transform Camera;
    [SerializeField] Transform portal1;
    [SerializeField] Transform portal2;

    private bool activated;

    // So that it refreshes when starting the level
    void OnEnable()
    {
        GameplayManager.Reset += DisableActivated;
        GameplayManager.StartGame += EnableActivated;
    }
    void OnDisable()
    {
        GameplayManager.Reset -= DisableActivated;
        GameplayManager.StartGame -= EnableActivated;
    }

    void DisableActivated()
    {
        activated = false;
    }
    void EnableActivated()
    {
        activated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera == null)
        {
            Camera = GameObject.FindWithTag("MainCamera").transform;
        }
        else
        {
            if (!activated)
            {
                Vector3 offset = Camera.position - portal1.position;
                transform.position = portal2.position + offset;

                transform.rotation = Camera.rotation;
            }
            else
            {
                Vector3 offset = Camera.position - portal2.position;
                transform.position = portal1.position + offset;

                transform.rotation = Camera.rotation;
            }
        }

    }
}
