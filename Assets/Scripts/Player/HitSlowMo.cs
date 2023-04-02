using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSlowMo : MonoBehaviour
{
    public float slowAmt;
    public float slowLength;

    public void EnableSlowMo()
    {
        Time.timeScale = slowAmt;
        Invoke("DisableSlowMo", slowLength);
    }

    void DisableSlowMo()
    {
        Time.timeScale = 1;
    }
}
