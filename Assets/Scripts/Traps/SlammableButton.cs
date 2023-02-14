using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlammableButton : MonoBehaviour
{
    public UnityEvent pressed = new UnityEvent();

    // Update is called once per frame
    public void Slam()
    {
        transform.localScale = new Vector3(transform.localScale.x, .1f, transform.localScale.z);
        pressed.Invoke();
    }
}
