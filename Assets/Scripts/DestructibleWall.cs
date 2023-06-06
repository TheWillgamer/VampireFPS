using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    private Renderer r;
    private Collider c;

    void Start()
    {
        r = GetComponent<Renderer>();
        c = GetComponent<Collider>();
    }

    void OnEnable()
    {
        GameplayManager.Reset += Reinstate;
    }
    void OnDisable()
    {
        GameplayManager.Reset -= Reinstate;
    }

    // Start is called before the first frame update
    public void Destroy()
    {
        if (r != null)
            r.enabled = false;
        if (c != null)
            c.enabled = false;
    }

    public void Reinstate()
    {
        if (r != null)
            r.enabled = true;
        if (c != null)
            c.enabled = true;
    }
}
