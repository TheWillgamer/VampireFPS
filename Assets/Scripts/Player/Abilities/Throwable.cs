using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public GameObject thrown;

    protected PlayerMovement pm;

    protected void Awake()
    {
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    public virtual void UseItem()
    {

    }

    public virtual void ThrowItem()
    {

    }
}
