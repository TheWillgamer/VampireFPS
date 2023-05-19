using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWorldSpaceCanvas : MonoBehaviour
{
    void OnEnable()
    {
        GameplayManager.Spawn += SetCanvas;
    }
    void OnDisable()
    {
        GameplayManager.Spawn -= SetCanvas;
    }

    private void SetCanvas()
    {
        GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }
}
