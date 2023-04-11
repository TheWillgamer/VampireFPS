using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationGate : MonoBehaviour
{
    [SerializeField]
    private Spawner[] requirements;

    private Renderer r;
    private Collider c;

    void Start()
    {
        r = GetComponent<Renderer>();
        c = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        bool deactivate = true;

        for (int i = 0; i < requirements.Length; i++)
        {
            if (requirements[i].alive)
                deactivate = false;
        }

        if (r != null)
            r.enabled = !deactivate;
        if (c != null)
            c.enabled = !deactivate;
    }
}
