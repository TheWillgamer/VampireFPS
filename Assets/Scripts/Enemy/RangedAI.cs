using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI : MonoBehaviour
{
    private Transform player;
    public Transform cbow;

    public float horRotSpeed;      // how fast enemies rotate towards the player horizontally
    public float verRotSpeed;      // how fast the crossbow rotates towards the player vertically
    public bool dead;



    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        player = GameObject.FindWithTag("Player").transform;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead)
        {
            var newRotation1 = Quaternion.LookRotation(player.position - transform.position);
            newRotation1.x = 0;
            newRotation1.z = 0;

            var newRotation2 = Quaternion.LookRotation(player.position - transform.position);
            newRotation2.z = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation1, Time.deltaTime * horRotSpeed);
            cbow.rotation = Quaternion.Slerp(cbow.rotation, newRotation2, Time.deltaTime * verRotSpeed);
        }
    }
}
