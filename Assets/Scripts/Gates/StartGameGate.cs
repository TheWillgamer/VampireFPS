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
        activated = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!activated && col.tag == "Player")
        {
            gm.DoStartGame();
            activated = true;
        }
    }
}
