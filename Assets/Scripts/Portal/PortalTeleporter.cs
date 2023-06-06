using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField] Transform receiver;
    [SerializeField] private float teleportDistance;
    private GameplayManager gm;
    private GameObject player;
    private bool active;

    void OnEnable()
    {
        GameplayManager.Spawn += Activate;
    }
    void OnDisable()
    {
        GameplayManager.Spawn -= Activate;
    }

    void Start()
    {
        gm = GameObject.FindWithTag("EventSystem").GetComponent<GameplayManager>();
        active = true;
    }

    void Update()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (!active || player == null)
            return;

        if (player.transform.position.z > (transform.position.z + teleportDistance))
        {
            player.transform.position = receiver.position + player.transform.position - transform.position;
            gm.DoStartGame();
            active = false;
        }
    }

    void Activate()
    {
        active = true;
    }
}
