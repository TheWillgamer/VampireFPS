using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_Brick : MonoBehaviour
{
    public GameObject brick;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.gameObject.GetComponent<Throw>().equipped == null)
        {
            Throwable th = Instantiate(brick, other.transform.position, other.transform.rotation).GetComponent<Throwable>();
            other.gameObject.GetComponent<Throw>().equipped = th;
            Destroy(gameObject);
        }
    }
}
