using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private bool triggerDialogue;
    [SerializeField] private DialogueSequence dialogueSequence;

    [SerializeField] private bool triggerInteractable;
    [SerializeField] private string interactableID;

    [SerializeField] private bool destroyOnInteract;

    [SerializeField] List<DialogueEvent> events = new List<DialogueEvent>();

    public void SetDialogueSequence(DialogueSequence dialogueSequence)
    {
        this.dialogueSequence = dialogueSequence;
    }

    public void OnInteract()
    {
        if (triggerDialogue)
        {
            if (dialogueSequence == null)
            {
                return;
            }

            DialogueManager.Instance.StartDialogueSequence(dialogueSequence);
        }

        if (triggerInteractable)
        {
            InteractableManager.Instance.DisplayInteractable(interactableID);
        }

        foreach (DialogueEvent e in events)
        {
            e.TriggerEvent();
        }

        if (destroyOnInteract)
        {
            Destroy(gameObject);
        }
    }
}
