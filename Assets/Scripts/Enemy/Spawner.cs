using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawning;
    private GameObject spawned;
    public bool alive = false;
    private EnemyHitDetection ehd = null;

    void Update()
    {
        if (spawned != null)
        {
            if (ehd == null)
                ehd = spawned.transform.GetChild(0).GetComponent<EnemyHitDetection>();

            alive = ehd.alive;
        }
            
    }

    public void Spawn()
    {
        spawned = Instantiate(spawning, transform.position, transform.rotation);
    }

    public void DestroySpawned()
    {
        ehd = null;
        if (spawned != null)
            Destroy(spawned);
    }
}
