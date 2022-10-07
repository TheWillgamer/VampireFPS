using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Ability
{
    [SerializeField] Transform rangedAttack;
    public AudioClip shot;
    public AudioSource sound;

    public override void UseAbility()
    {
        Instantiate(rangedAttack, pm.playerCam.transform.position, pm.playerCam.transform.rotation);
        sound.PlayOneShot(shot);
    }
}