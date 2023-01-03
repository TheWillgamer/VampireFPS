using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;
    public bool alive;

    //public Transform hp_bar;
    private Rigidbody rb;

    void Awake()
    {
        alive = true;
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0 && alive)
        {
            GetComponent<EnemyAI>().Death();
            alive = false;
        }
    }

    //void UpdateHealth()
    //{
    //    hp_bar.LookAt(GameObject.FindWithTag("MainCamera").transform.position);
    //    hp_bar.localScale = new Vector3((float)currentHealth / maxHealth, 0.1f, 0.1f);
    //}

    public void Knockback(float amount, Vector3 direction)
    {
        if (rb == null)
            return;

        float startTime = Time.time;

        rb.AddForce(direction * amount);
    }
}
