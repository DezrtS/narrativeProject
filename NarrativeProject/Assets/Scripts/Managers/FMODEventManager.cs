using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventManager : Singleton<FMODEventManager>
{
    //[field: Header("Music")]

    //[field: Header("Ambience")]

    //[field: Header("Player SFX")]

    //[field: Header("Character SFX")]

    //[field: Header("Environmental SFX")]

    [field: Header("UI SFX")]
    [field: SerializeField] public FMODAudioEvent Clue { get; private set; }
    [field: SerializeField] public FMODAudioEvent PageTurn { get; private set; }

    [field: Header("Misc SFX")]
    [field: SerializeField] public FMODAudioEvent Invalid { get; private set; }
    [field: SerializeField] public FMODAudioEvent Writing { get; private set; }
    
    private Dictionary<string, EventReference> eventIDLookup = new Dictionary<string, EventReference>();

    protected override void Awake()
    {
        base.Awake();

        eventIDLookup.Add(Clue.EventID, Clue.EventReference);
        eventIDLookup.Add(Invalid.EventID, Invalid.EventReference);
    }

    public EventReference GetEventReferenceFromID(string eventID)
    {
        if (eventIDLookup.TryGetValue(eventID, out var reference))
        {
            return reference;
        }

        return Invalid.EventReference;
    }
}

[Serializable]
public class FMODAudioEvent
{
    [SerializeField] private string eventID;
    [SerializeField] private EventReference eventReference;

    public string EventID {  get { return eventID; } }
    public EventReference EventReference { get {  return eventReference; } }
}