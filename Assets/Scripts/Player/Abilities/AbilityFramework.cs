using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityFramework : MonoBehaviour
{
    private int length = 3;

    // keeps track of cooldowns and inputs for each ability
    private Ability[] abilityList;
    private float[] offcd;
    private bool[] input;
    public Transform[] icons;

    protected PlayerMovement pm;    // to disable abilities when player is dead
    protected PlayerHealth ph;      // blood cost for abilities

    void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        ph = GetComponent<PlayerHealth>();
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
        AssignAbility(gameObject.GetComponent<Dash>(), 2);
    }

    // Update is called once per frame
    void Update()
    {
        // ability is activated;
        input[0] = Input.GetButton("Fire1");
        input[1] = Input.GetButtonDown("Fire2");
        input[2] = Input.GetButtonDown("Fire3");

        for (int i = 0; i < length; i++)
        {
            if (abilityList[i] != null && !pm.dead && !pm.paused)
            {
                InputCheck(i);
            }
        }
    }

    // Performs ability associated with input when valid
    void InputCheck(int i)
    {
        if (abilityList[i].charges > 0)
        {
            if (input[i])
            {
                abilityList[i].UseAbility();
                ph.TakeDamage(abilityList[i].bloodCost);

                if (abilityList[i].charges == abilityList[i].maxCharges)
                {
                    offcd[i] = Time.time + abilityList[i].cd;
                }

                abilityList[i].charges -= 1;
                
                //icons[i].GetChild(4).GetChild(1).GetComponent<Text>().text = abilityList[i].charges.ToString();
            }
        }

        if (Time.time > offcd[i] && abilityList[i].charges < abilityList[i].maxCharges)
        {
            abilityList[i].charges += 1;

            offcd[i] = Time.time + abilityList[i].cd;
        }

        if (abilityList[i].charges < abilityList[i].maxCharges)
        {
            icons[i].GetChild(1).GetComponent<Image>().fillAmount = (offcd[i] - Time.time) / abilityList[i].cd;
        }
    }

    void UpdateAbilities()
    {

    }

    public void AssignAbility(Ability a, int i)
    {
        abilityList[i] = a;
    }
}
