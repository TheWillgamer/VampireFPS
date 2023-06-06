using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    // 0 for normal, 1 for hard
    public void SetDifficulty(int i)
    {
        PlayerPrefs.SetInt("Difficulty", i);
        GameObject.FindWithTag("Player").GetComponent<DifficultyChooser>().ChangeDifficulty();
    }
}
