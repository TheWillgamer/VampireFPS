using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject[] choices;
    [SerializeField] private float typingSpeed;

    private Story currentStory;
    public bool dialogueIsPlaying;
    private Coroutine displayLineCoroutine;
    private TextMeshProUGUI[] choicesText;
    private bool pressed;
    private bool choiceIsMade;
    private bool canContinueToNext;     // if dialogue has finished typing
    private bool finishDialogue;        // if dialogue has finished typing

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("more than one DialogueManager found in the scene");
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        pressed = false;
        choiceIsMade = false;
        canContinueToNext = false;
        finishDialogue = false;

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            // When dialogue is finished
            if (canContinueToNext)
            {
                if (currentStory.currentChoices.Count == 0)
                    ContinueStory();
                else
                {
                    pressed = true;
                }
            }
            // to speed up the dialogue
            else
            {
                finishDialogue = true;
            }
        }

        if (pressed && choiceIsMade)
        {
            ContinueStory();
            pressed = false;
            choiceIsMade = false;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // Prevents lines from overlapping
            if (displayLineCoroutine != null)
                StopCoroutine(displayLineCoroutine);

            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            //dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";
        HideChoices();

        canContinueToNext = false;

        foreach (char letter in line.ToCharArray())
        {
            if (finishDialogue)
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        finishDialogue = false;
        canContinueToNext = true;
        DisplayChoices();
    }

    private void HideChoices()
    {
        foreach (GameObject choice in choices)
            choice.SetActive(false);
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        int index = 0;
        
        //enable and initialize choices
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        // set unused choices to false
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNext)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            choiceIsMade = true;
        }
    }
}
