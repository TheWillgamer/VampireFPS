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
    private bool won;

    protected PlayerMovement pm;
    private Rigidbody rb;

    // Blood ticking (decreases blood over time)
    public float tick_cd = 0.1f;
    private float offcd;

    public GameObject gameOverScreen;
    public GameObject victoryScreen;

    //Audio
    public AudioSource hit;
    public AudioSource dead;
    private bool deadPlayed;        // so that death audio doesn't play twice

    void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        deadPlayed = false;
        won = false;
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

        // when player wins
        if (won)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            victoryScreen.SetActive(true);
        }
    }

    // Reduces hp based on the parameter amount
    public void TakeDamage(int amt)
    {
        hp -= amt;
        UpdateUI();

        // Player dies if they take too much dmg
        if (hp <= 0 && !won)
        {
            pm.dead = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverScreen.SetActive(true);
            if (!deadPlayed)
            {
                dead.Play(0);
                deadPlayed = true;
            }
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
                TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
                hit.Play(0);
                direction = transform.position - other.transform.parent.transform.position;
                direction.Normalize();
                Knockback(600f, direction);
                StartCoroutine(Fade());
                Destroy(other.transform.parent.gameObject);
                break;
            case "Killbox":
                if(!won)
                    TakeDamage(2000);
                break;
            case "Winbox":
                won = true;
                break;
        }
    }

    public void ProjectileHit(int damage, Vector3 direction, float knockback)
    {
        TakeDamage(damage);
        hit.Play(0);
        rb.AddForce(direction * knockback + transform.up * knockback / 4);
        StartCoroutine(Fade());
    }

    public void Knockback(float amount, Vector3 direction)
    {
        rb.AddForce(direction * amount + transform.up * amount / 4);
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
