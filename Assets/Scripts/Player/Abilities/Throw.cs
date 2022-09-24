using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public Throwable equipped;
    [SerializeField] private Transform camera;
    [SerializeField] private float grabRadius;
    [SerializeField] private float grabDistance;
    [SerializeField] private GameObject grabText;

    // Start is called before the first frame update
    void Start()
    {
        equipped = null;
    }

    // Update is called once per frame
    void Update()
    {
        bool pressed = Input.GetButtonDown("Fire2");        // Grab/Throw button has been pressed
        bool canGrab = false;

        if (equipped != null)
        {
            if(pressed)
            {
                equipped.UseItem();
                equipped = null;
            }
        }
        else
        {
            Vector3 fwd = camera.TransformDirection(Vector3.forward);
            int layerMask = 1 << 7;
            RaycastHit hit;

            if (Physics.SphereCast(camera.position, grabRadius, fwd, out hit, grabDistance, layerMask))
            {
                canGrab = true;
                if (pressed)
                {
                    GameObject grabbed = hit.collider.gameObject;
                    equipped = Instantiate(grabbed.GetComponent<Grabbable>().holdingObject, transform.position, transform.rotation).GetComponent<Throwable>();
                    Destroy(grabbed);
                }
            }
        }
        grabText.SetActive(canGrab);
    }
}
