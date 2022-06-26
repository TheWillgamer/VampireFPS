using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "PlayerProjectile":
                Destroy(other.transform.parent.gameObject);
                break;
            case "EnemyProjectile":
                Destroy(other.transform.parent.gameObject);
                break;
        }
    }
}
