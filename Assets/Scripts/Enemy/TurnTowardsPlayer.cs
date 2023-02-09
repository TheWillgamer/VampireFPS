using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsPlayer : MonoBehaviour
{
    [SerializeField] private float turnSpeed;
    [SerializeField] private float alertRadius;     // how fast enemy turns towards the player
    [SerializeField] private bool disableVerticalTurn;     // how fast enemy turns towards the player
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

        if (disableVerticalTurn)
            relLoc = new Vector3(relLoc.x, 0, relLoc.z);
        
        if(active && relLoc.magnitude < alertRadius)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relLoc, Vector3.up), turnSpeed * Time.deltaTime);
        }
    }
}
