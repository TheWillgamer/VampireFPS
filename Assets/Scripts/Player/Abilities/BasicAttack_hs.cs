using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack_hs : Ability
{
    [SerializeField] private float maxRange;

    public AudioSource sound;

    public override void UseAbility()
    {
        RaycastHit hit;

        float range = maxRange;  // how far the beam goes

        if (Physics.Raycast(pm.playerCam.transform.position, pm.playerCam.transform.forward, out hit, maxRange))
        {
            range = hit.distance;
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.Log("Did not Hit");
        }

        sound.Play(0);
    }
}