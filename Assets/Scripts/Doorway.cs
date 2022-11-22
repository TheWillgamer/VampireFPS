using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Doorway : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private TMP_Text levelName;
    [SerializeField] private GameObject display;
    private PlayerData pd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (pd == null)
            {
                pd = GameObject.FindWithTag("EventSystem").GetComponent<SaveData>().playerData;

                levelName.text = pd.levels[level].name;
            }
            display.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            display.SetActive(false);
        }
    }
}
