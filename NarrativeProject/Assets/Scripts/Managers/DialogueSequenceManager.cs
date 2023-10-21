using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequenceManager : Singleton<DialogueSequenceManager>
{
    [SerializeField] private DialogueSequence[] dialogueSequences;

    private Dictionary<string, DialogueSequence> IDSequencePairs;

    protected override void Awake()
    {
        base.Awake();

        IDSequencePairs = new Dictionary<string, DialogueSequence>();

        foreach (DialogueSequence sequence in dialogueSequences)
        {
            IDSequencePairs.Add(sequence.SequenceID, sequence);
        }
    }

    public DialogueSequence GetSequence(string id)
    {
        if (IDSequencePairs.TryGetValue(id, out DialogueSequence sequence))
        {
            return sequence;
        }

        Debug.Log($"Sequence of id {id}");
        return null;
    }
}