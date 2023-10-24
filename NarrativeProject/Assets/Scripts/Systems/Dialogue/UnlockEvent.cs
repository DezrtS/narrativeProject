using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Event/Unlock Event")]
public class UnlockEvent : DialogueEvent
{
    [SerializeField] private string journalUnlockId;
    [SerializeField] private JournalPageData journalPageData;
    [SerializeField] private bool isItem = false;
    [SerializeField] private Condition condition;
    [SerializeField] private bool unlockConditionState = true;

    [SerializeField] private bool unlockJournalItem = false;
    [SerializeField] private bool forNPC = true;
    [SerializeField] private bool unlockDialogueCondition = false;


    public override void TriggerEvent()
    {
        if (unlockJournalItem)
        {
            if (forNPC)
            {
                NPCStatsManager.Instance.UnlockNPCStat(journalUnlockId);
            }
            else
            {
                JournalManager.Instance.AddJournalPage(journalPageData, isItem);
            }
        } 
        
        if (unlockDialogueCondition)
        {
            GameManager.Instance.SetCondition(condition, unlockConditionState);
        }
    }
}