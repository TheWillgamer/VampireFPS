using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPad : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float counterMovement;
    private bool disabled = false;
    private float tempMoveSpeed;
    private float tempMaxSpeed;
    private float tempCounterMovement;
    private PlayerMovement pm;

    void Start()
    {
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        tempMoveSpeed = pm.moveSpeed;
        tempMaxSpeed = pm.maxSpeed;
        tempCounterMovement = pm.counterMovement;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!disabled && other.tag == "Player")
        {
            pm.moveSpeed = moveSpeed;
            pm.maxSpeed = maxSpeed;
            pm.counterMovement = counterMovement;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!disabled && other.tag == "Player")
        {
            pm.moveSpeed = tempMoveSpeed;
            pm.maxSpeed = tempMaxSpeed;
            pm.counterMovement = tempCounterMovement;
        }
    }
}
