using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Level
{
    public string name;
    public bool completed;
    public float fastestTime;
    public int enemiesKilled;
    public int secretsFound;

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
    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/gamedata.txt";

        if (File.Exists(filePath))
        {
            playerData = Load();
        }
        else
        {
            playerData = new PlayerData();
            playerData.levelsCompleted = 0;
            playerData.timePlayed = 0f;
            playerData.totalEnemiesKilled = 0;
            playerData.totalSecretsFound = 0;
            playerData.levels = new Level[]
            {
                new Level ("Level0", false, 0, 0, 0),
                new Level ("Hellfall", false, 0, 0, 0)
            };

            Save(playerData);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Save(PlayerData data)
    {
        File.WriteAllText(filePath, data.ToString());
    }

    PlayerData Load()
    {
        PlayerData pd = new PlayerData();
        string data = File.ReadAllText(filePath);

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
            + "TimeSpentInLevels:" + timePlayed.ToString() + "\n"
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
