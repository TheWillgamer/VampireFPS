using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHead : EnemyAI
{
    public Transform headTracker;
    public Transform pullBackTracker;     // Where the head is pulled back before the throw
    public bool onHead;           // If full-sized head is on the golem
    public bool host;                               // if golem is alive
    [SerializeField] private float regenerateSpeed;           // How fast the head regenerates
    [SerializeField] private float throwForce;                // How fast the head is thrown
    public bool thrown;           // If the head was thrown by the golem
    private float distanceToTracker;
    private Rigidbody rb;
    [SerializeField] private int damage = 5;
    [SerializeField] private float damageRadius = 5;
    [SerializeField] private float knockback = 5;
    [SerializeField] private int playerDamage = 5;
    [SerializeField] private float playerKnockback = 5;
    [SerializeField] Transform explosion;

    public float timer;        // Timer for head animations
    private bool lifted;        // If head has been lifted
    private bool pulled;        // If head has been pulled back

    // Start is called before the first frame update
    void Start()
    {
        timer = Mathf.Infinity;
        lifted = false;
        pulled = false;
        rb = GetComponent<Rigidbody>();
        thrown = false;
        distanceToTracker = (headTracker.position - transform.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        active = true;
        timer += Time.deltaTime;
        if (!thrown && host)
        {
            if (onHead)
                transform.position = headTracker.position;
            
            else
            {
                if (headTracker.position.y - transform.position.y < 0f)
                {
                    onHead = true;
                    transform.localScale = new Vector3(100, 100, 92.5f);
                    transform.parent = transform.parent.parent;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, headTracker.position, regenerateSpeed * Time.deltaTime);
                    float scale = 1 - (headTracker.position - transform.position).magnitude / distanceToTracker;
                    transform.localScale = new Vector3(100, 100, 92.5f) * scale;
                }
            }

            transform.rotation = headTracker.rotation * Quaternion.Euler(-90, 0, 0);
        }
        else if (!host)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000f);
        }
        else
        {
            if (headTracker == null || pullBackTracker == null)
                return;

            if (!lifted && timer > .7f)
            {
                transform.position = Vector3.MoveTowards(transform.position, headTracker.position + new Vector3(0, .3f, 0), 1.5f * Time.deltaTime);
                transform.rotation = headTracker.rotation * Quaternion.Euler(-90, 0, 0);

                if (transform.position == headTracker.position + new Vector3(0, .3f, 0))
                    lifted = true;
            }
            else if (!lifted)
            {
                transform.position = headTracker.position;
                transform.rotation = headTracker.rotation * Quaternion.Euler(-90, 0, 0);
            }
            else if (!pulled)       // After head has been lifted
            {
                transform.position = Vector3.MoveTowards(transform.position, pullBackTracker.position, 2f * Time.deltaTime);
                transform.rotation = headTracker.rotation * Quaternion.Euler(-90, 0, 0);

                if (transform.position == pullBackTracker.position)
                    pulled = true;
            }
            else
            {
                rb.AddForce(Vector3.down * Time.deltaTime * 3000f);
            }
            
        }
    }

    public void Throw(Vector3 direction)
    {
        rb.AddForce(direction * throwForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            Death();
            //collision.transform.GetComponent<PlayerHealth>().ProjectileHit(damage, rb.velocity.normalized, knockback);
        }
    }

    public override void Death()
    {
        int layermask = 1 << 10;
        layermask = ~layermask;

        foreach (Collider col in Physics.OverlapSphere(transform.position, damageRadius, layermask))
        {
            if (col.tag == "Enemy")
            {
                EnemyHitDetection ehd = col.gameObject.GetComponent<EnemyHitDetection>();
                ehd.TakeDamage(damage);
                ehd.Knockback(knockback, (col.transform.position - transform.position).normalized);
            }
            else if (col.tag == "Player")
            {
                // Calculate Angle Between the collision point and the player
                Vector3 dir = col.transform.position - transform.position;
                // And finally we add force in the direction of dir and multiply it by force.
                //  This will push back the player
                //col.gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * playerKnockback);
                col.gameObject.GetComponent<PlayerHealth>().ProjectileHit(playerDamage, dir.normalized, playerKnockback);
            }
        }

        Instantiate(explosion, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
