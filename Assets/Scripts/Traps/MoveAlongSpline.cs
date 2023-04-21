using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongSpline : MonoBehaviour
{
    [SerializeField] private int speed;
    
    private Transform playerLoc;
    private Vector3 startingPos;
    private int next_point;     // where the sawblade is traveling to

    [SerializeField] private Transform[] points;        // points in the path of the sawblade
    
    void OnEnable()
    {
        GameplayManager.Spawn += ResetPosition;
    }
    void OnDisable()
    {
        GameplayManager.Spawn -= ResetPosition;
    }

    void Start()
    {
        startingPos = transform.position;
        next_point = 0;
    }

    void Update()
    {
        Debug.Log(transform.position);
        if (transform.position == points[next_point].position)
        {
            next_point++;
            if (next_point >= points.Length)
                next_point = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, points[next_point].position, speed * Time.deltaTime);
    }

    void ResetPosition()
    {
        transform.position = startingPos;
        next_point = 0;
    }
}
  