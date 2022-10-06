using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
