using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;
    public bool alive;

    public float shakeDuration;
    public float shakeMagnitude;

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
        else if (currentHealth > 0)
        {
            StartCoroutine(Shake(shakeDuration, shakeMagnitude));
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

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.GetChild(0).transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.GetChild(0).transform.localPosition = new Vector3(x, y, z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.GetChild(0).transform.localPosition = originalPos;
    }
}
