using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int hp;  //keeps track of player health
    public int maxHealth = 2000;
    public Transform hpMeter;

    protected PlayerMovement pm;

    // Blood ticking (decreases blood over time)
    public float tick_cd = 0.1f;
    private float offcd;

    public GameObject gameOverScreen;

    void Awake()
    {
        pm = GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHealth;
        offcd = Time.time;
    }

    void Update()
    {
        if(Time.time > offcd)
        {
            TakeDamage(1);
            offcd = Time.time + tick_cd;
        }
    }

    // Reduces hp based on the parameter amount
    public void TakeDamage(int amt)
    {
        hp -= amt;
        UpdateUI();

        // Player dies if they take too much dmg
        if (hp <= 0)
        {
            pm.dead = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverScreen.SetActive(true);
        }
    }

    private void UpdateUI()
    {
        hpMeter.GetComponent<Image>().fillAmount = (float)hp / maxHealth;
    }
}
