using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseScreen;
    public Transform player;
    public AudioSource footsteps;
    private GameplayManager gm;

    void Awake()
    {
        gm = GameObject.FindWithTag("EventSystem").GetComponent<GameplayManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        pauseScreen.SetActive(true);
        player.GetComponent<PlayerMovement>().paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        footsteps.Pause();
    }
    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        player.GetComponent<PlayerMovement>().paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        footsteps.Play();
    }
    public void RestartLevel()
    {
        ResumeGame();
        gm.RestartLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
