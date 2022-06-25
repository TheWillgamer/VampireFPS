using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    [SerializeField] float maxHealth = 100.0f;
    float currentHealth;

    //public Slider slider;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //slider.value = 1;
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
            case "Melee":
                if (gameObject.tag != "Player")
                {
                    TakeDamage(15f);
                    direction = transform.position - other.transform.parent.transform.parent.transform.position;
                    direction.y = 0;
                    direction.Normalize();
                    Knockback(400f, 0.1f, direction);
                }
                break;

            case "Projectile":
                TakeDamage(10f);
                direction = transform.position - other.transform.parent.transform.position;
                direction.Normalize();
                Knockback(200f, 0.1f, direction);
                Destroy(other.transform.parent.gameObject);
                break;
        }
    }

    void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void UpdateHealth()
    {
        //slider.value = currentHealth / maxHealth;
    }

    void Knockback(float amount, float length, Vector3 direction)
    {
        float startTime = Time.time;

        rb.AddForce(direction * amount + transform.up * amount);
    }
}
