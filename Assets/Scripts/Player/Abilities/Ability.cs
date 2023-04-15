using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public enum AbilityType
    {
        Melee,
        Projectile,
        Movement
    }

    public AbilityType abilityType;

    protected PlayerMovement pm;
    protected Rigidbody rb;
    protected Transform orientation;

    public float cd = 5f;
    public int charges = 1;
    public int maxCharges = 3;
    // public int bloodCost = 3;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        orientation = transform.GetChild(0).transform;
    }

    public virtual void UseAbility()
    {

    }
}
