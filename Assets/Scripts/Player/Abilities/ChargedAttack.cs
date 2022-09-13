using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedAttack : Ability
{
    [SerializeField] Transform rangedAttack;
    public AudioSource sound;
    public float chargeValue;
    [SerializeField] float minsize;
    [SerializeField] float maxsize;

    public override void UseAbility()
    {
        Transform temp = Instantiate(rangedAttack, pm.playerCam.transform.position, pm.playerCam.transform.rotation);
        float size = minsize + chargeValue * (maxsize - minsize);
        temp.localScale = new Vector3(size, size, size);

        ChargedPlayerProjectile cpp = temp.GetChild(0).GetComponent<ChargedPlayerProjectile>();
        cpp.chargeValue = chargeValue;

        sound.Play(0);
    }
}