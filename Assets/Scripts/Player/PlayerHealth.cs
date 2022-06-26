using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int hp;  //keeps track of player health
    public int maxHealth = 2000;
    public Transform hpMeter;
    public Image dmgScreen;

    protected PlayerMovement pm;
    private Rigidbody rb;

    // Blood ticking (decreases blood over time)
    public float tick_cd = 0.1f;
    private float offcd;

    public GameObject gameOverScreen;

    void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
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
        if (hp > maxHealth)
            hp = maxHealth;
    }

    private void UpdateUI()
    {
        hpMeter.GetComponent<Image>().fillAmount = (float)hp / maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 direction;

        switch (other.tag)
        {
            case "EnemyProjectile":
                TakeDamage(150);
                direction = transform.position - other.transform.parent.transform.position;
                direction.Normalize();
                Knockback(600f, direction);
                StartCoroutine(Fade());
                Destroy(other.transform.parent.gameObject);
                break;
        }
    }

    void Knockback(float amount, Vector3 direction)
    {
        float startTime = Time.time;

        rb.AddForce(direction * amount + transform.up * amount);
    }

    IEnumerator Fade()
    {
        Color c = dmgScreen.color;
        for (float alpha = .2f; alpha >= 0; alpha -= 0.001f)
        {
            c.a = alpha;
            dmgScreen.color = c;
            yield return null;
        }
    }
}
