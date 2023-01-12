using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenReset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.Reset += Destroy;
    }

    public void Destroy()
    {
        GameplayManager.Reset -= Destroy;
        Destroy(gameObject);
    }
}
