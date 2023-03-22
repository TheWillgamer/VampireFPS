using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public TMP_Text timerText;
    [SerializeField] private GameObject startGamePanel;
    private float timer;
    private bool active;

    [SerializeField]
    private bool hubArea;

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

    public delegate void OnStartGame();
    public static event OnStartGame StartGame;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        active = hubArea;
        RestartLevel();

        if (hubArea)
            Invoke(nameof(DoStartGame), 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        if (hubArea)
        {
            
        }
        else
        {
            if (active)
                timer += Time.deltaTime;
            DisplayTime();

            if (active && Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }
            else if (!active && Input.anyKeyDown)
            {
                DoStartGame();
            }
        }
    }

    public void RestartLevel()
    {
        DoReset();
        Invoke(nameof(DoSpawn), 0.01f);
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
        playerInstance.transform.GetChild(0).GetComponent<PlayerMovement>().paused = false;
        timer = 0f;
        active = false;

        if (Reset != null)
            Reset();
    }

    public void DoSpawn()
    {
        if (Spawn != null)
            Spawn();

        startGamePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DoStartGame()
    {
        startGamePanel.SetActive(false);
        Time.timeScale = 1f;
        playerInstance.transform.GetChild(0).GetComponent<PlayerMovement>().paused = false;
        active = true;

        if (StartGame != null)
            StartGame();
    }
}
