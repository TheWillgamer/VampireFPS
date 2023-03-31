using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxTransfer : MonoBehaviour
{
    [SerializeField] private Material otherSkybox;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = otherSkybox;
    }
}
