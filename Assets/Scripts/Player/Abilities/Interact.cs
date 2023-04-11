using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interact : MonoBehaviour
{
    [SerializeField] private float zoomTime;
    [SerializeField] private float interactDistance;
    [SerializeField] private GameObject interactText;
    private GameObject UI;

    private PlayerMovement pm;
    private Rigidbody rb;

    private Transform newLoc;
    private Transform npc;
    private bool activated;
    private bool reset;
    private Transform camera;
    private Vector3 tempPosition;
    private Quaternion tempRotation;
    private float startTime;
    private bool dialogueStarted;

    private TextAsset dialogue;     // What the NPC says

    void Start()
    {
        activated = false;
        reset = false;
        camera = GameObject.FindWithTag("CameraMan").transform;
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        UI = GameObject.FindWithTag("PlayerUI");
        dialogueStarted = false;
    }

    void Update()
    {
        if (camera == null)
            camera = GameObject.FindWithTag("CameraMan").transform;

        CheckInteract();

        if (activated)
        {
            camera.position = Vector3.Lerp(tempPosition, newLoc.position, (Time.time - startTime) / zoomTime);
            camera.rotation = Quaternion.Slerp(tempRotation, newLoc.rotation, (Time.time - startTime) / zoomTime);
            if (!dialogueStarted && Time.time - startTime > zoomTime)
            {
                dialogueStarted = true;
                DialogueManager.GetInstance().EnterDialogueMode(dialogue);
            }

            // Reset Camera when dialogue is over
            if (dialogueStarted && !DialogueManager.GetInstance().dialogueIsPlaying)
            {
                transform.position = newLoc.position - 2f * Vector3.up;
                Vector3 relLoc = npc.position - transform.position;
                relLoc = new Vector3(relLoc.x, 0, relLoc.z);
                transform.GetChild(0).localRotation = Quaternion.LookRotation(relLoc, Vector3.up);

                activated = false;
                reset = true;
                startTime = Time.time;
                dialogueStarted = false;
            }
        }
        else if (reset)
        {
            camera.position = Vector3.Lerp(newLoc.position, transform.GetChild(1).position, (Time.time - startTime) / zoomTime);
            camera.rotation = Quaternion.Slerp(newLoc.rotation, transform.GetChild(0).rotation, (Time.time - startTime) / zoomTime);
            if (Time.time - startTime > zoomTime)
            {
                camera.GetComponent<MoveCamera>().active = true;
                reset = false;
                UI.SetActive(true);
                pm.active = true;
            }
        }
    }

    void CheckInteract()
    {
        bool pressed = Input.GetKeyDown(KeyCode.F);
        if (activated && Input.GetKeyDown(KeyCode.G)) {
            transform.position = newLoc.position - 2f * Vector3.up;
            Vector3 relLoc = npc.position - transform.position;
            relLoc = new Vector3(relLoc.x, 0, relLoc.z);
            transform.GetChild(0).localRotation = Quaternion.LookRotation(relLoc, Vector3.up);

            activated = false;
            reset = true;
            startTime = Time.time;
            dialogueStarted = false;
        }
        bool canTalk = false;

        Vector3 fwd = camera.TransformDirection(Vector3.forward);
        int layerMask = 1 << 11;
        RaycastHit hit;

        if (!DialogueManager.GetInstance().dialogueIsPlaying && Physics.Raycast(camera.position, fwd, out hit, interactDistance, layerMask))
        {
            canTalk = true;
            if (pressed)
            {
                activated = true;
                camera.GetComponent<MoveCamera>().active = false;
                tempPosition = camera.position;
                tempRotation = camera.rotation;
                npc = hit.transform;
                dialogue = npc.GetComponent<NPCdialogue>().inkJson;
                
                newLoc = npc.GetChild(0);
                startTime = Time.time;
                UI.SetActive(false);

                rb.velocity = new Vector3(0f, 0f, 0f);
                transform.position = newLoc.position - 2f * Vector3.up;
                pm.active = false;

                Vector3 relLoc = hit.transform.position - transform.position;
                relLoc = new Vector3(relLoc.x, 0, relLoc.z);
                transform.GetChild(0).localRotation = Quaternion.LookRotation(relLoc, Vector3.up);
                pm.xRotation = 0f;
            }
        }

        interactText.SetActive(canTalk);
    }
}
