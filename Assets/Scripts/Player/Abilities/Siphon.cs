using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Siphon : MonoBehaviour
{
    public Transform camera;
    public float siphonDistance;
    public GameObject siphonText;
    private bool canSiphon;
    public int siphonAmount;        // how much healing
    public Animator anim;
    public AudioSource sound;

    void Awake()
    {
        canSiphon = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fwd = camera.TransformDirection(Vector3.forward);
        RaycastHit hit;

        canSiphon = false;
        if (Physics.Raycast(camera.position, fwd, out hit, siphonDistance) && hit.collider.tag == "Edible")
            canSiphon = hit.collider.transform.parent.gameObject.GetComponent<RangedAI>().dead;     // checks if enemy corpse is in front of player

        siphonText.SetActive(canSiphon);

        if (canSiphon && Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("siphoning");
            sound.Play(0);
            Destroy(hit.collider.transform.parent.gameObject);
            GetComponent<PlayerHealth>().TakeDamage(-siphonAmount);
        }
    }
}