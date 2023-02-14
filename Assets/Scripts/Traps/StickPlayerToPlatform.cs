using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlayerToPlatform : MonoBehaviour
{
    private bool cancellingStick;
    private bool stuck;
    private PlayerMovement pm;

    void Start()
    {
        stuck = false;
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!stuck)
            {
                pm = other.gameObject.GetComponent<PlayerMovement>();
                pm.prb = GetComponent<Rigidbody>();
                pm.onMovingPlatform = true;
            }
            cancellingStick = false;
            CancelInvoke(nameof(StopStick));

            float delay = 2f;
            if (!cancellingStick)
            {
                cancellingStick = true;
                Invoke(nameof(StopStick), Time.deltaTime * delay);
            }
        }
    }

    private void StopStick()
    {
        pm.onMovingPlatform = false;
        stuck = false;
    }
}
