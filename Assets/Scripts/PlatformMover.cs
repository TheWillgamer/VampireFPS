using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    float Timer;
    public float DirectionTimer = 3f;
    int DirectionRight;
    public float PlatformSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        Timer = Time.time + DirectionTimer;
 
        DirectionRight = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Timer < Time.time) {
            Timer = Time.time + DirectionTimer;
            if (DirectionRight == 1)
            DirectionRight = -1;
            else 
            DirectionRight = 1;
        }
        transform.position += Vector3.right * DirectionRight * PlatformSpeed * Time.deltaTime;
    }
}