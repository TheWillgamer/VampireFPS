using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DifficultyChooser : MonoBehaviour
{
    private int difficulty;
    private PlayerHealth ph;

    public float[] difficultyScalings;

    // Start is called before the first frame update
    void Start()
    {
        ph = GetComponent<PlayerHealth>();

        if (PlayerPrefs.HasKey("Difficulty"))
            difficulty = PlayerPrefs.GetInt("Difficulty");
        else
        {
            PlayerPrefs.SetInt("Difficulty", 1);
            difficulty = 1;
        }

        ph.ChangeTickRate(difficultyScalings[difficulty]);
    }

    public void ChangeDifficulty()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty");
        ph.ChangeTickRate(difficultyScalings[difficulty]);
    }
}
