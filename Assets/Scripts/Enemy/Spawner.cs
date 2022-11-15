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
            {
                ehd = spawned.GetComponent<EnemyHitDetection>();
                if (ehd == null)
                    ehd = spawned.transform.GetChild(0).GetComponent<EnemyHitDetection>();
            }

            if (ehd == null)
                alive = false;
            else
                alive = ehd.alive;
        }
        else
            alive = false;
            
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
