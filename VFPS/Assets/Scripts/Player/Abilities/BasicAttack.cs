using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Ability
{
    [SerializeField] Transform rangedAttack;
    [SerializeField] Transform rangedSpawn;

    public override void UseAbility()
    {
        Instantiate(rangedAttack, rangedSpawn.position, rangedSpawn.rotation);
    }
}