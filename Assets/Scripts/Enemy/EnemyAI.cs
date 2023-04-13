using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public bool active;

    void OnEnable()
    {
        GameplayManager.StartGame += ActivateEnemy;
        GameplayManager.EndGame += DisableEnemy;
    }
    void OnDisable()
    {
        GameplayManager.StartGame -= ActivateEnemy;
        GameplayManager.EndGame -= DisableEnemy;
    }

    void ActivateEnemy()
    {
        active = true;
    }
    void DisableEnemy()
    {
        active = false;
    }

    void Start()
    {
        active = true;
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
