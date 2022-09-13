using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public Transform bullet;
    bool reached = false;       // when trail catches up with bullet
    public float speed;         // how fast trail goes towards bullet

    // Update is called once per frame
    void Update()
    {
        if (!reached)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, Time.deltaTime * speed);
            if (transform.localPosition == Vector3.zero)
                reached = true;
        }
    }
}
