using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseScreen;
    private GameplayManager gm;
    private GameObject PlayerUI;
    private bool paused;

    void Awake()
    {
        gm = GetComponent<GameplayManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    void PauseGame()
    {
        paused = true;
        pauseScreen.SetActive(true);
        PlayerUI = GameObject.FindWithTag("PlayerUI");
        PlayerUI.SetActive(false);
        //player.GetComponent<PlayerMovement>().paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        //footsteps.Pause();
    }
    public void ResumeGame()
    {
        paused = false;
        pauseScreen.SetActive(false);
        PlayerUI.SetActive(true);
        //player.GetComponent<PlayerMovement>().paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        //footsteps.Play();
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
