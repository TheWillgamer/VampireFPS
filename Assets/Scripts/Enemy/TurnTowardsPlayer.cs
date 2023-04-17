using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsPlayer : MonoBehaviour
{
    [SerializeField] private float turnSpeed;
    [SerializeField] private float alertRadius;     // how fast enemy turns towards the player
    [SerializeField] private bool disableVerticalTurn;     // how fast enemy turns towards the player
    private GameObject player;
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

        if (!active || player == null)
            return;
        
        Vector3 relLoc = player.transform.position - transform.position;

        if (disableVerticalTurn)
            relLoc = new Vector3(relLoc.x, 0, relLoc.z);
        
        if(relLoc.magnitude < alertRadius)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relLoc, Vector3.up), turnSpeed * Time.deltaTime);
        }
    }
}
