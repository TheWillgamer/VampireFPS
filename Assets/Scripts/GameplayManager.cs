using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public TMP_Text timerText;
    private float timer;
    private bool timerActive;

    // Everything that needs to be spawned in a level
    [SerializeField]
    private Spawner[] spawns;
    
    [SerializeField]
    private GameObject player;
    private GameObject playerInstance;

    public delegate void OnReset();
    public static event OnReset DoReset;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        timerActive = true;
        Reset();
        SpawnFrom(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
            timer += Time.deltaTime;
        DisplayTime();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Reset();
            Invoke(nameof(sp), 0.01f);
            //SpawnFrom(0);
        }
    }

    void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void sp()
    {
        SpawnFrom(0);
    }

    // Spawns all enemies from the index
    public void SpawnFrom(int index)
    {
        for(int i = index; i < spawns.Length; i++)
        {
            spawns[i].Spawn();
        }
    }

    public void Reset()
    {
        for (int i = 0; i < spawns.Length; i++)
        {
            spawns[i].DestroySpawned();
        }

        if (playerInstance != null)
            Destroy(playerInstance);
        playerInstance = Instantiate(player, transform.position, transform.rotation);

        if (DoReset != null)
            DoReset();
    }
}
