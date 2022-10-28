using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawning;
    private GameObject spawned;

    public void Spawn()
    {
        spawned = Instantiate(spawning, transform.position, transform.rotation);
    }

    public void DestroySpawned()
    {
        if (spawned != null)
            Destroy(spawned);
    }
}
