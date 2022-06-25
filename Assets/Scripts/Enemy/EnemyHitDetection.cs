using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHitDetection : MonoBehaviour
{
    [SerializeField] float maxHealth = 100.0f;
    float currentHealth;

    public Slider slider;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        slider.value = 1;
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
                TakeDamage(4f);
                direction = transform.position - other.transform.parent.transform.position;
                direction.Normalize();
                Knockback(600f, 0, direction);
                Destroy(other.transform.parent.gameObject);
                break;
        }
    }

    void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.mass = 10f;
        }
    }

    void UpdateHealth()
    {
        slider.value = currentHealth / maxHealth;
    }

    void Knockback(float amount, float length, Vector3 direction)
    {
        float startTime = Time.time;

        rb.AddForce(direction * amount + transform.up * amount);
    }
}
