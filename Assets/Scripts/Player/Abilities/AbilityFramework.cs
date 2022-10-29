using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityFramework : MonoBehaviour
{
    private int length = 4;

    // keeps track of cooldowns and inputs for each ability
    private Ability[] abilityList;
    private float[] offcd;
    private bool[] input;
    public Transform[] icons;

    private float basicatktimer = 0f;       // temp variable to fix animations

    protected PlayerMovement pm;    // to disable abilities when player is dead
    protected PlayerHealth ph;      // blood cost for abilities
    public Animator anim;

    public AudioSource Blast_recharged;
    public AudioSource Dash_recharged;

    void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        ph = GetComponent<PlayerHealth>();
        basicatktimer = Time.time;
    }

    void Start()
    {
        abilityList = new Ability[length];
        offcd = new float[length];
        input = new bool[length];

        for (int i = 0; i < length; i++)
        {
            abilityList[i] = null;
            offcd[i] = 0f;
            input[i] = false;
        }

        AssignAbility(gameObject.GetComponent<BasicAttack>(), 0);
        AssignAbility(gameObject.GetComponent<Melee>(), 1);
        AssignAbility(gameObject.GetComponent<Dash>(), 2);
        AssignAbility(gameObject.GetComponent<Blast>(), 3);
    }

    // Update is called once per frame
    void Update()
    {
        // ability is activated;
        input[0] = Input.GetButton("Fire1");
        input[1] = Input.GetButtonDown("Melee");
        input[2] = Input.GetButtonDown("Fire3");
        input[3] = Input.GetButtonDown("Fire4");

        anim.SetBool("shooting", input[0] && !pm.dead && !pm.paused);

        // when fire button is first used
        if (Input.GetButtonDown("Fire1"))
        {
            basicatktimer = Time.time + 0f;
        }

        for (int i = 0; i < length; i++)
        {
            if (abilityList[i] != null && !pm.dead && !pm.paused)
            {
                InputCheck(i);
            }
        }

        UpdateAbilities();
    }

    // Performs ability associated with input when valid
    void InputCheck(int i)
    {
        if (abilityList[i].charges > 0)
        {
            if (input[i])
            {
                if ((i == 0 && Time.time > basicatktimer) || i > 0)
                {
                    abilityList[i].UseAbility();
                    ph.TakeDamage(abilityList[i].bloodCost);

                    if (abilityList[i].charges == abilityList[i].maxCharges)
                    {
                        offcd[i] = Time.time + abilityList[i].cd;
                    }

                    abilityList[i].charges -= 1;
                }
            }
        }

        if (Time.time > offcd[i] && abilityList[i].charges < abilityList[i].maxCharges)
        {
            abilityList[i].charges += 1;
            if (i == 2)
                Dash_recharged.Play(0);
            if (i == 3)
                Blast_recharged.Play(0);

            offcd[i] = Time.time + abilityList[i].cd;
        }
    }

    void UpdateAbilities()
    {
        icons[3].GetComponent<Image>().fillAmount = 1 - (offcd[3] - Time.time) / abilityList[3].cd;
        icons[5].GetComponent<Image>().fillAmount = 0;
        if (abilityList[3].charges == abilityList[3].maxCharges)
        {
            icons[3].GetComponent<Image>().fillAmount = 1;
            icons[5].GetComponent<Image>().fillAmount = 1;
        }

        if (abilityList[2].charges > 0)
        {
            icons[0].GetComponent<Image>().fillAmount = 0.5f;
            icons[1].GetComponent<Image>().fillAmount = 1;
            icons[2].GetComponent<Image>().fillAmount = 1 - (offcd[2] - Time.time) / abilityList[2].cd;
            icons[4].GetComponent<Image>().fillAmount = (1 - (offcd[2] - Time.time) / abilityList[2].cd) / 2;
        }
        else
        {
            icons[0].GetComponent<Image>().fillAmount = (1 - (offcd[2] - Time.time) / abilityList[2].cd) / 2;
            icons[1].GetComponent<Image>().fillAmount = 1 - (offcd[2] - Time.time) / abilityList[2].cd;
            icons[2].GetComponent<Image>().fillAmount = 0;
            icons[4].GetComponent<Image>().fillAmount = 0;
        }

        if (abilityList[2].charges == abilityList[2].maxCharges)
        {
            icons[2].GetComponent<Image>().fillAmount = 1;
            icons[4].GetComponent<Image>().fillAmount = 0.5f;
        }
    }

    public void AssignAbility(Ability a, int i)
    {
        abilityList[i] = a;
    }
}
