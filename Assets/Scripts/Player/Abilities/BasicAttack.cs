using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Ability
{
    [SerializeField] Transform rangedAttack;
    [SerializeField] Transform rangedSpawn_l;
    [SerializeField] Transform rangedSpawn_r;
    public AudioSource sound;
    private bool left = false;

    public override void UseAbility()
    {
        Transform rangedSpawn;
        if (left)
        {
            rangedSpawn = rangedSpawn_l;
            left = false;
        }
        else
        {
            rangedSpawn = rangedSpawn_r;
            left = true;
        }

        PlayerProjectile proj = Instantiate(rangedAttack, rangedSpawn.position, rangedSpawn.rotation).GetChild(0).GetComponent<PlayerProjectile>();
        proj.start = rangedSpawn;

        int layerMask = 1 << 2;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(pm.playerCam.transform.position, pm.playerCam.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            proj.end = hit.point;
            Debug.Log(hit.collider.gameObject.name);
        }
        else
        {
            proj.end = pm.playerCam.transform.position + pm.playerCam.transform.forward * 10000;
        }
        Debug.Log(proj.end);
        sound.Play(0);
    }
}