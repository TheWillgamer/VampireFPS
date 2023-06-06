using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int loadNext;

    private int currentScene;
    
    void Start()
    {
        currentScene = 0;
        loadNext = -1;
    }

    public void ShowStats(int scene)
    {
        loadNext = scene;
    }

    public void Load()
    {
        if (currentScene != 0)
            SceneManager.UnloadScene(currentScene);
        if (loadNext < 0)
            return;
        SceneManager.LoadScene(loadNext + 1, LoadSceneMode.Additive);
        currentScene = loadNext + 1;
    }

    public void Load(int scene)
    {
        if (currentScene != 0)
            SceneManager.UnloadScene(currentScene);
        SceneManager.LoadScene(scene + 1, LoadSceneMode.Additive);
        currentScene = scene + 1;
    }

    public void LoadNext()
    {
        SceneManager.UnloadScene(currentScene);
        if (currentScene == 8)
            currentScene = 0;
        currentScene++;
        SceneManager.LoadScene(currentScene, LoadSceneMode.Additive);
    }
}
