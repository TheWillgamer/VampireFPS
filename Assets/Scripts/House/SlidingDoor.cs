using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    private bool opening;
    [SerializeField] private Transform door;
    [SerializeField] private Transform endPos;
    [SerializeField] private float OCtime;      // open/close time (how long to open/close the door)
    private Vector3 startPos;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startPos = door.position;
        startTime = Time.time - OCtime;
        opening = false;
    }

    void Update()
    {
        if (opening)
            door.position = Vector3.Lerp(startPos, endPos.position, (Time.time - startTime) / OCtime);
        else
            door.position = Vector3.Lerp(endPos.position, startPos, (Time.time - startTime) / OCtime);
    }

    public void OpenDoor()
    {
        startTime = (Time.time - startTime > OCtime) ? Time.time : (2 * Time.time - startTime - OCtime);
        opening = true;
    }

    public void CloseDoor()
    {
        startTime = (Time.time - startTime > OCtime) ? Time.time : (2 * Time.time - startTime - OCtime);
        opening = false;
    }
}
