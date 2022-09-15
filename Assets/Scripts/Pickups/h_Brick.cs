using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class h_Brick : Throwable
{
    public override void UseItem()
    {
        Instantiate(thrown, pm.playerCam.transform.position, pm.playerCam.transform.rotation);
        Destroy(gameObject);
    }

    public override void ThrowItem()
    {
        Instantiate(thrown, pm.playerCam.transform.position, pm.playerCam.transform.rotation);
        Destroy(gameObject);
    }
}
