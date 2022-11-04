using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashFOV : MonoBehaviour
{
    [Header("field of view")]
    [SerializeField] Camera camera;
    [SerializeField] bool dynamicFOV;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    DynamicFOV();
        
    }

    void DynamicFOV()
    {
        if(Input.GetKey(KeyCode.LeftShift) && dynamicFOV == true)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 100, 10f * Time.deltaTime);
        }
        else
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 80, 10 * Time.deltaTime);

        }
    }
}
