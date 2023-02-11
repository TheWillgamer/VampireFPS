using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    private bool hit;

    void Start()
    {
        hit = false;
    }

    public void Hit()
    {
        if (!hit)
        {
            hit = true;
            Debug.Log("obj was hit");
        }
    }
}
