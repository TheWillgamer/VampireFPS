using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] private float zoomOutTime;
    [SerializeField] private Transform newLoc;
    private bool activated;
    private Transform camera;
    private Vector3 tempPosition;
    private Quaternion tempRotation;
    private float startTime;

    // So that it refreshes when starting the level
    void OnEnable()
    {
        GameplayManager.Reset += DisableActivated;
    }
    void OnDisable()
    {
        GameplayManager.Reset -= DisableActivated;
    }

    void DisableActivated()
    {
        activated = false;
    }

    void Start()
    {
        activated = false;
    }

    void Update()
    {
        if (activated)
        {
            camera.position = Vector3.Lerp(tempPosition, newLoc.position, (Time.time - startTime) / zoomOutTime);
            camera.rotation = Quaternion.Slerp(tempRotation, newLoc.rotation, (Time.time - startTime) / zoomOutTime);
        }
            
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            camera = GameObject.FindWithTag("CameraMan").transform;
            camera.GetComponent<MoveCamera>().active = false;
            //StartCoroutine(camera.GetComponent<MoveCamera>().FadeOut());
            activated = true;
            tempPosition = camera.position;
            tempRotation = camera.rotation;
            startTime = Time.time;
            Destroy(other.gameObject);
        }
    }
}
