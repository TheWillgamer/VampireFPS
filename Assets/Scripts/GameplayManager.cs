using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public TMP_Text timerText;
    private float timer;
    private bool active;
    
    [SerializeField]
    private GameObject player;
    private GameObject playerInstance;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject victoryScreen;

    public delegate void OnReset();
    public static event OnReset Reset;

    public delegate void OnSpawn();
    public static event OnSpawn Spawn;

    public delegate void OnStartGame();
    public static event OnStartGame StartGame;

    public delegate void OnEndGame();
    public static event OnEndGame EndGame;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        RestartLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
            timer += Time.deltaTime;
        DisplayTime();

        if (active && Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        DoReset();
        Invoke(nameof(DoSpawn), 0.5f);
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
        restartButton.interactable = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        deathScreen.SetActive(false);
        victoryScreen.SetActive(false);

        if (Reset != null)
            Reset();
    }

    public void DoSpawn()
    {
        if (Spawn != null)
            Spawn();

    }

    // When player has left the starting area
    public void DoStartGame()
    {
        active = true;
        restartButton.interactable = true;

        if (StartGame != null)
            StartGame();
    }
    
    // When player reached the goal
    public void DoEndGame()
    {
        active = false;
        GameObject.FindWithTag("PlayerUI").SetActive(false);
        restartButton.interactable = false;
        Invoke("ShowVictoryScreen", 1.2f);

        if (EndGame != null)
            EndGame();
    }

    // When player reached the goal
    public void DoPlayerDeath()
    {
        active = false;
        GameObject.FindWithTag("PlayerUI").SetActive(false);
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
