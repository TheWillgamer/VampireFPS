using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public TMP_Text timerText;
    private float timer;
    private bool timerActive;

    // Everything that needs to be spawned in a level
    [SerializeField]
    private Spawner[] spawns;
    
    [SerializeField]
    private GameObject player;
    private GameObject playerInstance;

    public delegate void OnReset();
    public static event OnReset Reset;

    public delegate void OnSpawn();
    public static event OnSpawn Spawn;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        timerActive = true;
        DoReset();
        Invoke(nameof(DoSpawn), 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
            timer += Time.deltaTime;
        DisplayTime();

        if (Input.GetKeyDown(KeyCode.J))
        {
            DoReset();
            Invoke(nameof(DoSpawn), 0.01f);
        }
    }

    void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void DoReset()
    {
        if (playerInstance != null)
            Destroy(playerInstance);
        playerInstance = Instantiate(player, transform.position, transform.rotation);

        if (Reset != null)
            Reset();
    }

    public void DoSpawn()
    {
        if (Spawn != null)
            Spawn();
    }
}
