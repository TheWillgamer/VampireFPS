using System.Collections;
using UnityEngine;

public class Dash : Ability
{
    public float dashSpeed = 2000f;
    public float dashTime = 0.1f;

    public override void UseAbility()
    {
        pm.disableCM = true;
        //ps.canLook = false;
        //ps.canMove = false;
        //ps.SwitchToThird();

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
        rb.velocity = rb.velocity / 5;

        pm.disableCM = false;
    }
}