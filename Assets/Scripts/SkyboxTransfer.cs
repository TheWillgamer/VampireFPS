using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkyboxTransfer : MonoBehaviour
{
    [SerializeField] private Material otherSkybox;
    [SerializeField] private Color otherSkyboxColor;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = otherSkybox;
        RenderSettings.ambientLight = otherSkyboxColor;
    }
}
