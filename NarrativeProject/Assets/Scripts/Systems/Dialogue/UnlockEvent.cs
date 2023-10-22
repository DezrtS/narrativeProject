using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Unlock Event")]
public class UnlockEvent : DialogueEvent
{
    [SerializeField] private string unlockId;

    [SerializeField] private bool isForJournal = true;

    public override void TriggerEvent()
    {
        NPCStatsManager.Instance.UnlockNPCStat(unlockId);
    }
}