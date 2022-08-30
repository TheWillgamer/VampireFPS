using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : Ability
{
    public float dashSpeed = 2000f;
    public float activateTime = 0.1f;
    public AudioSource sound;

    public override void UseAbility()
    {
        //sound.Play(0);

        Invoke(nameof(Activate), activateTime);
    }

    void Activate()
    {
        rb.AddForce(-pm.playerCam.transform.forward * dashSpeed);
    }
}
