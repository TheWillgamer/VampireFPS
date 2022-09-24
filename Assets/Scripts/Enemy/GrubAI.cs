using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrubAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float alertRadius;     // how fast enemy turns towards the player
    private Transform player;
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relLoc = player.position - transform.position;

        if (active && relLoc.magnitude < alertRadius)
        {
            transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
