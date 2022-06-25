using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 30.0f;
    [SerializeField] float aliveTime = 5.0f;
    float deathTime;

    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + aliveTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
        if (Time.time > deathTime)
        {
            Destroy(gameObject);
        }
    }
}
