using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float speed;

    void OnCollisionStay(Collision col)
    {
        if (col.rigidbody != null)
        {
            if (col.transform.position.z - transform.position.z > 0)
                col.rigidbody.velocity = new Vector3(0f, speed, 0f);
            else
                col.rigidbody.velocity = new Vector3(0f, -speed, 0f);
        }
    }
}
