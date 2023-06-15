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
    public bool invulnerability;

    protected PlayerMovement pm;
    private Rigidbody rb;

    // Blood ticking (decreases blood over time)
    public bool healthDrainActive;
    public float tick_cd = 0.1f;
    private float offcd;

    private GameplayManager gm;

    //Audio
    public AudioSource hit;
    public AudioSource dead;
    private bool deadPlayed;        // so that death audio doesn't play twice

    void OnEnable()
    {
        GameplayManager.StartGame += EnableHPDrain;
    }
    void OnDisable()
    {
        GameplayManager.StartGame -= EnableHPDrain;
    }

    void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();

        deadPlayed = false;
        invulnerability = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHealth;
        offcd = Time.time;
        gm = GameObject.FindWithTag("EventSystem").GetComponent<GameplayManager>();
    }

    void Update()
    {
        if (healthDrainActive && Time.time > offcd)
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
        if (hp <= 0 && !deadPlayed)
        {
            pm.dead = true;
            gm.DoPlayerDeath();
            dead.Play(0);
            deadPlayed = true;
        }
        if (hp > maxHealth)
            hp = maxHealth;
    }

    private void UpdateUI()
    {
        RectTransform rt = hpMeter.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2 ((1000f * (float)hp / maxHealth) - 1000f, rt.anchoredPosition.y);
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
                TakeDamage(10000);
                break;
        }
    }

    public void ProjectileHit(int damage, Vector3 direction, float knockback)
    {
        if (invulnerability)
            return;
        TakeDamage(damage);
        hit.Play(0);
        rb.AddForce(direction * knockback + transform.up * knockback / 4);
        if (!deadPlayed)
            StartCoroutine(Fade());
    }

    public void Knockback(float amount, Vector3 direction)
    {
        rb.AddForce(direction * amount + transform.up * amount / 4);
    }

    public IEnumerator Fade()
    {
        Color c = dmgScreen.color;
        for (float alpha = .2f; alpha >= 0; alpha -= 0.001f)
        {
            c.a = alpha;
            dmgScreen.color = c;
            yield return null;
        }
    }

    public void EnableHPDrain()
    {
        healthDrainActive = true;
        offcd = Time.time + tick_cd;
    }

    public void ChangeTickRate(float rate)
    {
        tick_cd = rate;
        offcd = Time.time;
    }
}
