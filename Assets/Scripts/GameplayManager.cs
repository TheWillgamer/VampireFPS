using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private float startTime = -1f;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        Debug.Log(startTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(startTime >= 0)
        {
            Debug.Log(Time.time - startTime);
        }
    }

    void DisplayTime()
    {

    }
}
