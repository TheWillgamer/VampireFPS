using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetTrigger : MonoBehaviour
{
    public UnityEvent hit = new UnityEvent();

    public void Hit()
    {
        hit.Invoke();
    }
}
