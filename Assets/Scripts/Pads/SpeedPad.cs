using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPad : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    private bool disabled = false;
    private float tempMoveSpeed;
    private float tempMaxSpeed;
    private PlayerMovement pm;

    void Start()
    {
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        tempMoveSpeed = pm.moveSpeed;
        tempMaxSpeed = pm.maxSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!disabled && other.tag == "Player")
        {
            pm.moveSpeed = moveSpeed;
            pm.maxSpeed = maxSpeed;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!disabled && other.tag == "Player")
        {
            pm.moveSpeed = tempMoveSpeed;
            pm.maxSpeed = tempMaxSpeed;
        }
    }
}
