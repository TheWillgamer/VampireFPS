using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHead : MonoBehaviour
{
    [SerializeField] private Transform headTracker;
    [SerializeField] private bool active;           // If full-sized head is on the golem
    public bool host;                               // if golem is alive
    [SerializeField] private float regenerateSpeed;           // How fast the head regenerates
    private bool thrown;           // If the head was thrown by the golem
    private float distanceToTracker;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        thrown = false;
        distanceToTracker = (headTracker.position - transform.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (!thrown && host)
        {
            if (active)
                transform.position = headTracker.position;
            
            else
            {
                if ((headTracker.position - transform.position).magnitude < .01f)
                {
                    active = true;
                    transform.localScale = new Vector3(100, 100, 92.5f);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, headTracker.position, regenerateSpeed * Time.deltaTime);
                    float scale = 1 - (headTracker.position - transform.position).magnitude / distanceToTracker;
                    transform.localScale = new Vector3(100, 100, 92.5f) * scale;
                }
            }

            transform.rotation = headTracker.rotation * Quaternion.Euler(-90, 0, 0);
        }
        else if (!host)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 2000f);
        }
    }
}
