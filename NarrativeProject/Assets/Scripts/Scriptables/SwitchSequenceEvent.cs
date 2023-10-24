using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Event/Switch Sequence Event")]
public class SwitchSequenceEvent : DialogueEvent
{
    [SerializeField] private DialogueSequence dialogueSequence;

    public override void TriggerEvent()
    {
        DialogueManager.Instance.SwitchToDialogueSequence(dialogueSequence);
    }
}
