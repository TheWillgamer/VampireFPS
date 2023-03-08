using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DifficultyChooser : MonoBehaviour
{
    public int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            difficulty = PlayerPrefs.GetInt("Difficulty"); ;
        }
        else
            PlayerPrefs.SetInt("Difficulty", 1);
    }
}
