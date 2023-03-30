using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private Transform endPos;
    [SerializeField] private float OCtime;      // open/close time (how long to open/close the door)
    private Vector3 startPos;
    private Quaternion startRot;
    private float startTime;

    private bool opening;

    void Start()
    {
        startTime = Time.time - OCtime;
        opening = false;
        startPos = door.position;
        startRot = door.rotation;
    }

    void Update()
    {
        if (opening)
        {
            door.position = Vector3.Lerp(startPos, endPos.position, (Time.time - startTime) / OCtime);
            door.rotation = Quaternion.Slerp(startRot, endPos.rotation, (Time.time - startTime) / OCtime);
        }
        else
        {
            door.position = Vector3.Lerp(endPos.position, startPos, (Time.time - startTime) / OCtime);
            door.rotation = Quaternion.Slerp(endPos.rotation, startRot, (Time.time - startTime) / OCtime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            startTime = (Time.time - startTime > OCtime) ? Time.time : (2 * Time.time - startTime - OCtime);
            opening = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            startTime = (Time.time - startTime > OCtime) ? Time.time : (2 * Time.time - startTime - OCtime);
            opening = false;
        }
    }
}