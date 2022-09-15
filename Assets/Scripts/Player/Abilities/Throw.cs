using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public Throwable equipped;

    // Start is called before the first frame update
    void Start()
    {
        equipped = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (equipped != null && Input.GetButtonDown("Fire2"))
        {
            equipped.UseItem();
            equipped = null;
        }
    }
}
