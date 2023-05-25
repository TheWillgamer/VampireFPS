using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    private int currentScene;
    
    void Start()
    {
        //SceneManager.LoadScene(1, LoadSceneMode.Additive);
        currentScene = 0;
    }

    void Update()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                Load(i);
            }
        }
    }

    public void Load(int scene)
    {
        if (currentScene != 0)
            SceneManager.UnloadScene(currentScene);
        SceneManager.LoadScene(scene + 1, LoadSceneMode.Additive);
        currentScene = scene + 1;
    }
}
