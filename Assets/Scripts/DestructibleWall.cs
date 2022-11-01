using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    void Start()
    {
        GameplayManager.DoReset += Reinstate;
    }

    // Start is called before the first frame update
    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    public void Reinstate()
    {
        gameObject.SetActive(true);
    }
}
