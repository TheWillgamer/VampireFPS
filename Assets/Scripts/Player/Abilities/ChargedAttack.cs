using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedAttack : Ability
{
    [SerializeField] Transform rangedAttack;
    [SerializeField] Transform rangedSpawn;
    public AudioSource sound;
    [SerializeField] float minsize;
    [SerializeField] float maxsize;

    public override void UseAbility()
    {
        Transform temp = Instantiate(rangedAttack, rangedSpawn.position, rangedSpawn.rotation);
        temp.localScale = new Vector3(maxsize, maxsize, maxsize);
        sound.Play(0);
    }
}