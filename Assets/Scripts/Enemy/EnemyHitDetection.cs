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

    private void OnTriggerEnter(Collider other)
    {
        Vector3 direction;

        switch (other.tag)
        {
            case "PlayerProjectile":
                TakeDamage(4);
                direction = transform.position - other.transform.parent.transform.position;
                direction.Normalize();
                Knockback(600f, 0, direction);
                Destroy(other.transform.parent.gameObject);
                break;
        }
    }

    void TakeDamage(int dmg)
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
            }
            currentHealth = 0;
            rb.constraints = RigidbodyConstraints.None;
            rb.mass = 10f;
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

    void Knockback(float amount, float length, Vector3 direction)
    {
        float startTime = Time.time;

        rb.AddForce(direction * amount + transform.up * amount);
    }
}
