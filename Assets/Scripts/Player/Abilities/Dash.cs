using System.Collections;
using UnityEngine;

public class Dash : Ability
{
    public float dashSpeed = 2000f;
    public float dashTime = 0.1f;
    public float endDashSlowdown = 0.5f;
    public AudioSource sound;

    public override void UseAbility()
    {
        pm.disableCM = true;
        pm.disableAR = true;
        sound.Play(0);

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (pm.y != 0 || pm.x == 0)      // For FOV change
            pm.dynamicFOV = true;

        if (pm.y == 0 && pm.x == 0)
        {
            rb.AddForce(orientation.transform.forward * dashSpeed);
        }
        else
        {
            Vector2 temp = new Vector2(pm.x, pm.y);
            temp.Normalize();
            rb.AddForce(orientation.transform.forward * temp.y * dashSpeed);
            rb.AddForce(orientation.transform.right * temp.x * dashSpeed);
        }

        Invoke(nameof(EndDash), dashTime);
    }

    void EndDash()
    {
        rb.velocity = rb.velocity * endDashSlowdown;

        pm.disableCM = false;
        pm.disableAR = false;
        pm.dynamicFOV = false;
    }
}