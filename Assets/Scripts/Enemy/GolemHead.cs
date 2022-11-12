using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHead : MonoBehaviour
{
    [SerializeField] private Transform headTracker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = headTracker.position;
    }
}
