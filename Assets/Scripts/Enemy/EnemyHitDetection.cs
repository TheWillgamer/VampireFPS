using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;
    bool alive;

    public Transform hp_bar;
    private Rigidbody rb;
    private PlayerHealth ph;
    public int lifestealAmt;

    public GameObject deathParticles;

    void Awake()
    {
        alive = true;
        rb = GetComponent<Rigidbody>();
        ph = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            if (alive)
            {
                Destroy(transform.GetChild(1).gameObject);
                Destroy(transform.GetChild(3).gameObject);
                GetComponent<RangedAI>().dead = true;
                alive = false;
                Instantiate(deathParticles, transform.position, Quaternion.identity);
                rb.constraints = RigidbodyConstraints.None;
                rb.mass = 10f;
            }
            currentHealth = 0;
        }
        else
        {
            ph.TakeDamage(-lifestealAmt);
        }
    }

    void UpdateHealth()
    {
        hp_bar.LookAt(GameObject.FindWithTag("MainCamera").transform.position);
        hp_bar.localScale = new Vector3((float)currentHealth / maxHealth, 0.1f, 0.1f);
    }

    public void Knockback(float amount, Vector3 direction)
    {
        float startTime = Time.time;

        rb.AddForce(direction * amount);
    }
}
