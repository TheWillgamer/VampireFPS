using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameGate : MonoBehaviour
{
    private GameplayManager gm;

    void Start()
    {
        gm = GameObject.FindWithTag("EventSystem").GetComponent<GameplayManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            gm.DoStartGame();
        }
    }
}
