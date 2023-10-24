using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public delegate void DialogueCallback();

    public event DialogueCallback OnDialogueStart;
    public event DialogueCallback OnDialogueEnd;

    public event DialogueCallback OnDialogueSequenceEnd;

    [SerializeField] private GameObject dialogueObject;

    [SerializeField] private GameObject choiceBackground;
    [SerializeField] private GameObject choiceButton;
    [SerializeField] private GameObject choiceHolder;

    [SerializeField] private GameObject dialogueHolder;
    [SerializeField] private TextMeshProUGUI dialogueSpeakerUI;
    [SerializeField] private TextMeshProUGUI dialogueUI;

    [SerializeField] private DialogueSequence tempSequence;

    private List<GameObject> activeChoices = new List<GameObject>();

    private DialogueSequence currentDialogueSequence;
    private int currentDialogueIndex = 0;

    private bool isTyping = false;
    private bool isMaintaining = false;
    private bool isChoosing = false;
    private int typingSpeedMultiplier = 1;

    [SerializeField] private Color defaultTextColor;

    private Color invisibleTextColor = new Color(0, 0, 0, 0);

    private DialogueEffectManager dialogueEffectManager = new DialogueEffectManager();

    private Animator choiceBackgroundAnimator;
    private Animator dialogueHolderAnimator;

    private bool switchedDialogue = false;

    protected override void Awake()
    {
        base.Awake();

        choiceBackgroundAnimator = choiceBackground.GetComponent<Animator>();
        dialogueHolderAnimator = dialogueHolder.GetComponent<Animator>();
    }

    private void Start()
    {
        //StartDialogueSequence(tempSequence);
    }

    public void StartDialogueSequence(DialogueSequence dialogueSequence)
    {
        PlayerController.Instance.dialogueMode = true;

        if (!dialogueObject.activeSelf)
        {
            dialogueObject.SetActive(true);
        }

        if (currentDialogueSequence == null)
        {
            //dialogueSpeakerUI.enabled = true;
            //dialogueUI.enabled = true;
            OnDialogueStart?.Invoke();
            currentDialogueSequence = dialogueSequence;
            currentDialogueIndex = 0;

            dialogueHolderAnimator.SetBool("Show", true);
            DisplayDialogue(dialogueSpeakerUI, dialogueUI, currentDialogueSequence.DialogueNodes[currentDialogueIndex].DialogueLine);
        } 
        else
        {
            Debug.LogError("Cannot start new dialogue while another dialogue sequence is running");
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartDialogueSequence(tempSequence);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !PlayerController.Instance.journalMode)
        {
            AdvanceDialogue();
        }

        if (isTyping || isMaintaining)
        {
            dialogueEffectManager.ApplyEffects(dialogueUI);
        }
    }

    public void AdvanceDialogue()
    {
        if (!dialogueObject.activeSelf)
        {
            return;
        }

        if (currentDialogueSequence != null && !isChoosing)
        {
            if (isTyping)
            {
                SkipDialogue();
                return;
            }

            currentDialogueIndex++;

            //currentDialogueSequence.DialogueNodes[currentDialogueIndex].ActivateEvents();

            DisplayDialogue(dialogueSpeakerUI, dialogueUI, currentDialogueSequence.DialogueNodes[currentDialogueIndex].DialogueLine);
        } 
        else
        {
            return;
        }
    }

    public void SkipDialogue()
    {
        if (currentDialogueSequence.CanSkipSequenceDialogue)
        {
            typingSpeedMultiplier = 20;
        }
    }

    public void SkipDialogueSequence()
    {

    }

    public void SwitchToDialogueSequence(DialogueSequence dialogueSequence)
    {
        Debug.Log("Switching Dialogue");

        switchedDialogue = true;

        if (dialogueSequence != null)
        {
            StopAllCoroutines();

            if (isMaintaining)
            {
                isMaintaining = false;
            }

            currentDialogueSequence = dialogueSequence;
            currentDialogueIndex = 0;

            DisplayDialogue(dialogueSpeakerUI, dialogueUI, currentDialogueSequence.DialogueNodes[currentDialogueIndex].DialogueLine);
        }
        else
        {
            EndDialogueSequence();
            return;
        }
    }

    public void EndDialogueSequence()
    {
        if (currentDialogueSequence != null)
        {
            if (isMaintaining)
            {
                isMaintaining = false;
                StopAllCoroutines();
            }

            dialogueHolderAnimator.SetBool("Show", false);

            //dialogueSpeakerUI.enabled = false;
            //dialogueUI.enabled = false;
            OnDialogueEnd?.Invoke();
            currentDialogueSequence = null;
            currentDialogueIndex = 0;
            //dialogueObject.SetActive(false);
        }

        PlayerController.Instance.dialogueMode = false;
    }

    public void DisplayDialogue(TextMeshProUGUI speakerUI, TextMeshProUGUI textUI, string text)
    {
        if (isMaintaining)
        {
            isMaintaining = false;
            StopAllCoroutines();
        }

        isTyping = true;

        speakerUI.text = currentDialogueSequence.DialogueNodes[currentDialogueIndex].DialogueSpeaker.name;

        textUI.text = dialogueEffectManager.FormatDialogueText(text);
        textUI.color = invisibleTextColor;

        dialogueEffectManager.SaveOriginalTextPositions(textUI);

        StartCoroutine(TypewriterDisplayDialogue(textUI, 0));
    }

    public void RunEventsAndChoices()
    {
        currentDialogueSequence.DialogueNodes[currentDialogueIndex].ActivateEvents();

        if (switchedDialogue)
        {
            return;
        }

        if (currentDialogueIndex == currentDialogueSequence.DialogueNodes.Count - 1 && currentDialogueSequence.ChoiceNode.Choices.Count > 0)
        {
            DisplayChoices();
        }
    }

    public void DisplayChoices()
    {
        if (isChoosing)
        {
            return;
        }

        isChoosing = true;

        StartCoroutine(DisplayChoicesAnimation());
    }

    public void ActivateChoice(Choice choice)
    {
        if (isChoosing && currentDialogueSequence.ChoiceNode.Choices.Contains(choice))
        {
            isChoosing = false;
            choiceBackgroundAnimator.SetBool("Show", false);

            if (choice.DialogueEvent != null)
            {
                choice.DialogueEvent.TriggerEvent();
            }

            if (choice.ChoiceDialogueSequence != null)
            {
                DestroyActiveChoices();
                SwitchToDialogueSequence(choice.ChoiceDialogueSequence);
                return;
            }

            DestroyActiveChoices();
            EndDialogueSequence();
        }
    }

    public void DestroyActiveChoices()
    {
        int count = 0;

        while (activeChoices.Count > 0 && count < 100)
        {
            GameObject choice = activeChoices[0];
            activeChoices.RemoveAt(0);
            Destroy(choice);
            count++;
        }
    }

    public void MaintainDialogue(TextMeshProUGUI textUI)
    {
        isMaintaining = true;

        StartCoroutine(MaintainDialogueText(textUI));
    }

    public IEnumerator TypewriterDisplayDialogue(TextMeshProUGUI textUI, int currentIndex)
    {

        while (textUI.text.Length > currentIndex)
        {
            for (int i = 0; i < currentIndex; i++)
            {
                char indexCharacter = textUI.text[currentIndex];

                if (!CharacterTypingData.skipCharacters.Contains(indexCharacter))
                {
                    dialogueEffectManager.ApplyColorEffect(textUI, currentIndex, defaultTextColor);
                }
            }

            char currentCharacter = textUI.text[currentIndex];
            float timeBetweenCharacters = 0.1f;

            if (CharacterTypingData.skipCharacters.Contains(currentCharacter))
            {
                timeBetweenCharacters = 0;
            }
            else
            {
                dialogueEffectManager.ApplyScreenShakeEffect(currentIndex);
                dialogueEffectManager.ApplyAudioEffect(currentIndex);

                if (CharacterTypingData.longPauseCharacters.Contains(currentCharacter))
                {
                    timeBetweenCharacters = CharacterTypingData.longPauseTime;
                }
                else if (CharacterTypingData.shortPauseCharacters.Contains(currentCharacter))
                {
                    timeBetweenCharacters = CharacterTypingData.shortPauseTime;
                }

                dialogueEffectManager.ApplyColorEffect(textUI, currentIndex, defaultTextColor);

                textUI.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            }

            yield return new WaitForSeconds(timeBetweenCharacters / typingSpeedMultiplier / dialogueEffectManager.ApplyTypingSpeedEffect(currentIndex));

            currentIndex++;
        }

        isTyping = false;
        typingSpeedMultiplier = 1;

        MaintainDialogue(textUI);

        RunEventsAndChoices();

        switchedDialogue = false;
    }

    public IEnumerator MaintainDialogueText(TextMeshProUGUI textUI)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            for (int i = 0; i < textUI.text.Length; i++)
            {
                char indexCharacter = textUI.text[i];

                if (!CharacterTypingData.skipCharacters.Contains(indexCharacter))
                {
                    dialogueEffectManager.ApplyColorEffect(textUI, i, defaultTextColor);
                }
            }

            textUI.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
    }

    public IEnumerator DisplayChoicesAnimation()
    {
        choiceBackgroundAnimator.SetBool("Show", true);
        yield return new WaitForSeconds(1);

        int offset = 0;

        foreach (Choice choice in currentDialogueSequence.ChoiceNode.Choices)
        {
            if (choice.DialogueCondition != null)
            {
                if (!choice.DialogueCondition.CanTrigger())
                {
                    continue;
                }
            }

            GameObject button = Instantiate(choiceButton, choiceHolder.transform);
            activeChoices.Add(button);
            button.GetComponent<DialogueChoice>().Choice = choice;
            RectTransform rect = button.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(0, offset, 0);
            button.GetComponentInChildren<TextMeshProUGUI>().text = choice.ChoicePreview;
            offset = offset + 100;

            yield return new WaitForSeconds(0.1f);
        }
    }
}

public static class CharacterTypingData
{
    public static float longPauseTime = 0.5f;
    public static float shortPauseTime = 0.25f;

    public static List<char> longPauseCharacters = new List<char>() { '.', '!', '?' };
    public static List<char> shortPauseCharacters = new List<char>() { ',' };
    public static List<char> skipCharacters = new List<char>() { ' ' };
}