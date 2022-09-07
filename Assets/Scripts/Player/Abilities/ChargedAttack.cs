using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedAttack : Ability
{
    [SerializeField] Transform rangedAttack;
    [SerializeField] Transform rangedSpawn;
    public AudioSource sound;
    public float chargeValue;
    [SerializeField] float minsize;
    [SerializeField] float maxsize;

    public override void UseAbility()
    {
        Transform temp = Instantiate(rangedAttack, rangedSpawn.position, rangedSpawn.rotation);
        float size = minsize + chargeValue * (maxsize - minsize);
        temp.localScale = new Vector3(size, size, size);

        ChargedPlayerProjectile cpp = temp.GetChild(0).GetComponent<ChargedPlayerProjectile>();
        cpp.chargeValue = chargeValue;
        cpp.Launch();

        sound.Play(0);
    }
}