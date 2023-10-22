using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Journals/Journal NPC Page Data")]
public class JournalNPCPageData : JournalPageData
{
    [SerializeField] private NPCs NPC;

    public override string GetJournalDataContent()
    {
        return base.GetJournalDataContent() + FormatNPCStats();
    }

    public string FormatNPCStats()
    {
        string stats = "";

        List<NPCStat> NPCStats = NPCStatsManager.Instance.GetNPCStats(NPC);

        if (NPCStats == null)
        {
            return stats;
        }

        foreach (NPCStat stat in NPCStats)
        {
            stats += stat.StatContent + "<br>";
        }

        return stats;
    }
}
