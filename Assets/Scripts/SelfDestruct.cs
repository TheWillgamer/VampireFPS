using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float aliveTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destruct", aliveTime);
    }

    void Destruct()
    {
        Destroy(gameObject);
    }
}
