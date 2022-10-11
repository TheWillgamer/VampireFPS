using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformLauncher : MonoBehaviour
{
    [SerializeField] private float launchSpeed;
    [SerializeField] private float resetSpeed;
    [SerializeField] private float wait;      // How long to wait before resetting
    [SerializeField] private Transform endPoint;

    private Transform playerLoc;
    private Vector3 startPoint;
    private float offcd;
    private PlayerMovement pm;
    private Rigidbody rb;
    private bool startwait;
    private bool readyToLaunch;     // when platform first reaches the point
    private bool resetting;
    private float prevMag;          // magnitude of the last time the platform was moving

    void Start()
    {
        startPoint = transform.position;
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        offcd = Time.time;
        readyToLaunch = true;
        prevMag = Mathf.Infinity;
    }

    void Update()
    {
        if (!resetting && (transform.position - endPoint.position).magnitude > prevMag)
        {
            if (!startwait)
            {
                pm.disableCM = true;
                offcd = Time.time + wait;
                startwait = true;
                rb.velocity = Vector3.zero;
            }
            if(Time.time > offcd)
            {
                rb.velocity = (startPoint - transform.position).normalized * resetSpeed;
                resetting = true;
                startwait = false;
            }
        }
        if (resetting && (transform.position - startPoint).magnitude < 1f)
        {
            resetting = false;
            readyToLaunch = true;
        }
        prevMag = (transform.position - endPoint.position).magnitude;
    }

    void OnCollisionEnter(Collision other)
    {
        if (readyToLaunch && other.gameObject.tag == "Player")
        {
            rb.velocity = (endPoint.position - transform.position).normalized * launchSpeed;
            other.gameObject.GetComponent<Rigidbody>().velocity += (endPoint.position - transform.position).normalized * launchSpeed;
            readyToLaunch = false;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            pm.disableCM = false;
        }
    }
}
