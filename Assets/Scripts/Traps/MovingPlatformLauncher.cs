using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformLauncher : MonoBehaviour
{
    [SerializeField] private float launchSpeed;
    [SerializeField] private float resetSpeed;
    [SerializeField] private float wait;      // How long to wait before resetting
    [SerializeField] private bool launcher;   // If the platform will launch the player
    [SerializeField] private Transform endPoint;

    private Transform playerLoc;
    private Vector3 startPoint;
    private float offcd;
    private PlayerMovement pm;
    private Rigidbody rb;
    private bool startwait;
    private bool readyToLaunch;     // when platform first reaches the point
    private bool resetting;
    private bool launched;          // keeps track if the player was launched
    private float prevMag;          // magnitude of the last time the platform was moving

    void Start()
    {
        startPoint = transform.position;
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        offcd = Time.time;
        readyToLaunch = true;
        resetting = false;
        prevMag = Mathf.Infinity;
        launched = false;
    }

    void Update()
    {
        if (!resetting && (transform.position - endPoint.position).magnitude > prevMag && prevMag < 10f)
        {
            if (!startwait)
            {
                if (launcher && !launched)       // allows player to be launched{
                {
                    pm.disableCM = true;
                    launched = true;
                }
                    
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
            launched = false;
            readyToLaunch = true;
        }
        prevMag = (transform.position - endPoint.position).magnitude;
    }

    void OnCollisionEnter(Collision other)
    {
        if (readyToLaunch && other.gameObject.tag == "Player")
        {
            readyToLaunch = false;
            rb.velocity = (endPoint.position - transform.position).normalized * launchSpeed;

            if (launcher)       // Increases player velocity if launcher
                other.gameObject.GetComponent<Rigidbody>().velocity += rb.velocity;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (launcher && other.gameObject.tag == "Player")
        {
            pm.disableCM = false;
        }
    }
}
