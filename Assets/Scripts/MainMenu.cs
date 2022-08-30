using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Tutorial()
    {
        SceneManager.LoadScene("Level_1");
    }
    public void Level1()
    {
        SceneManager.LoadScene("Level_2");
    }
    public void Level2()
    {
        SceneManager.LoadScene("Level_3.2");
    }
    public void Level3()
    {
        SceneManager.LoadScene("Level_4-2");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
