using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public bool active;
    public GameObject player;

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
        //active = false;
    }

    void Start()
    {
        active = true;
        player = GameObject.FindWithTag("Player");
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
