using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public struct Level
{
    string name;
    bool completed;
    float fastestTime;
    int enemiesKilled;
    int secretsFound;

    public Level(string n, bool c, float t, int e, int s)
    {
        name = n;
        completed = c;
        fastestTime = t;
        enemiesKilled = e;
        secretsFound = s;
    }

    public override string ToString()
    {
        return name + "," + completed.ToString() + "," + fastestTime.ToString() + "," + enemiesKilled.ToString() + "," + secretsFound.ToString();
    }
}

public class SaveData : MonoBehaviour
{
    string filePath;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/gamedata.json";

        if (File.Exists(filePath))
        {

        }
        else
        {
            PlayerData data = new PlayerData();
            data.levelsCompleted = 0;
            data.timePlayed = 0f;
            data.totalEnemiesKilled = 0;
            data.totalSecretsFound = 0;
            data.levels = new Level[]
            {
                new Level ("Level0", false, 0, 0, 0),
                new Level ("Hellfall", false, 0, 0, 0)
            };

            Debug.Log(Load(data.ToString()));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Save(PlayerData data)
    {

    }

    PlayerData Load(string data)
    {
        PlayerData pd = new PlayerData();

        string[] dataChunks = data.Split("\n");
        pd.levelsCompleted = Convert.ToInt32(dataChunks[0].Split(":")[1]);
        pd.timePlayed = float.Parse(dataChunks[1].Split(":")[1]);
        pd.totalEnemiesKilled = Convert.ToInt32(dataChunks[2].Split(":")[1]);
        pd.totalSecretsFound = Convert.ToInt32(dataChunks[3].Split(":")[1]);
        pd.levels = new Level[dataChunks.Length - 7];

        for (int i = 6; i < dataChunks.Length - 1; i++)
        {
            string[] chunks = dataChunks[i].Split(",");
            pd.levels[i - 6] = new Level(chunks[0], chunks[1]=="1", float.Parse(chunks[2]), Convert.ToInt32(chunks[3]), Convert.ToInt32(chunks[4]));
        }

        return pd;
    }
}

[Serializable]
public class PlayerData
{
    public int levelsCompleted;
    public float timePlayed;
    public int totalEnemiesKilled;
    public int totalSecretsFound;
    public Level[] levels;

    public override string ToString()
    {
        string toReturn = "LevelsCompleted:" + levelsCompleted.ToString() + "\n"
            + "TimePlayed:" + timePlayed.ToString() + "\n"
            + "EnemiesKilled:" + totalEnemiesKilled.ToString() + "\n"
            + "SecretsFound:" + totalSecretsFound.ToString() + "\n\n";

        toReturn += "LevelName,LevelCompletion,FastestTime,EnemiesKilled,SecretsFound\n";
        foreach (Level level in levels)
        {
            toReturn += level.ToString() + "\n";
        }

        return toReturn;
    }
}
