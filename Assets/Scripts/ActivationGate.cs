using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationGate : MonoBehaviour
{
    [SerializeField]
    private Spawner[] requirements;

    // Update is called once per frame
    void Update()
    {
        bool deactivate = true;

        for (int i = 0; i < requirements.Length; i++)
        {
            if (requirements[i].alive)
                deactivate = false;
        }

        gameObject.SetActive(!deactivate);
    }
}
