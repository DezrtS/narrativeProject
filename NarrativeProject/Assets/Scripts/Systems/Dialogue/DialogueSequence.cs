using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    [SerializeField] private DialogueSpeaker dialogueSpeaker;
    [SerializeField] [TextArea(3, 9)] private string dialogueLine;

    public DialogueSpeaker DialogueSpeaker { get { return dialogueSpeaker; } }
    public string DialogueLine { get { return dialogueLine; } }

    [SerializeField] private List<DialogueEvent> dialogueEvents;

    public void ActivateEvents()
    {
        foreach (DialogueEvent dialogueEvent in dialogueEvents)
        {
            dialogueEvent.TriggerEvent();
            Debug.Log("Triggering Dialogue Event");
        }
    }
}

[Serializable]
public class ChoiceNode
{
    [SerializeField] private List<Choice> choices;

    public List<Choice> Choices { get { return choices; } }
}

[Serializable]
public class Choice
{
    [SerializeField] private string choicePreview;
    [SerializeField] private DialogueEvent dialogueEvent;
    [SerializeField] private DialogueCondition dialogueCondition;
    [SerializeField] private DialogueSequence choiceDialogueSequence;

    public string ChoicePreview { get { return choicePreview; } }
    public DialogueEvent DialogueEvent { get { return dialogueEvent; } }
    public DialogueCondition DialogueCondition { get { return dialogueCondition; } }
    public DialogueSequence ChoiceDialogueSequence { get { return choiceDialogueSequence; } }
}

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    [SerializeField] private string sequenceID;
    [SerializeField] private bool canSkipSequenceDialogue = true;
    [SerializeField] private bool canSkipSequence = false;
    [SerializeField] private List<DialogueNode> dialogueSequence = new List<DialogueNode>();
    [SerializeField] private ChoiceNode choiceNode;

    public string SequenceID { get { return sequenceID; } }
    public bool CanSkipSequenceDialogue { get { return canSkipSequenceDialogue; } }
    public bool CanSkipSequence { get { return canSkipSequence; } }
    public List<DialogueNode> DialogueNodes { get { return dialogueSequence; } }
    public ChoiceNode ChoiceNode { get { return choiceNode; } }
}