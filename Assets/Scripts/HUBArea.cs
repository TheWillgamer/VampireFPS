using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBArea : MonoBehaviour
{
    [SerializeField]
    private GameObject[] areas;
    private int currentArea;

    void Start()
    {
        currentArea = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (currentArea == 0)
                SetArea(1);
            else if (currentArea == 1)
                SetArea(2);
            else
                SetArea(0);
        }
    }

    void SetArea(int area)
    {
        areas[currentArea].SetActive(false);
        currentArea = area;
        areas[area].SetActive(true);
    }
}
