using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameGate : MonoBehaviour
{
    private GameplayManager gm;
    private bool activated;

    void Start()
    {
        gm = GameObject.FindWithTag("EventSystem").GetComponent<GameplayManager>();
    }

    // So that it refreshes when starting the level
    void OnEnable()
    {
        GameplayManager.Reset += DisableActivated;
    }
    void OnDisable()
    {
        GameplayManager.Reset -= DisableActivated;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!activated && col.tag == "Player")
        {
            gm.DoStartGame();
            activated = true;
        }
    }

    void DisableActivated()
    {
        activated = false;
    }
}
