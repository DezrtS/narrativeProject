using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCs
{
    Amy,
    NPC2,
    NPC3,
    NPC4,
    NPC5,
    NPC6,
    NPC7,
}

public class NPCStatsManager : Singleton<NPCStatsManager>
{
    [SerializeField] private List<NPCStat> NPCStats = new List<NPCStat>();

    private List<NPCStatCollection> statCollections = new List<NPCStatCollection>();


    public List<NPCStat> GetNPCStats(NPCs NPC)
    {
        foreach (NPCStatCollection collection in statCollections)
        {
            if (collection.CollectionNPC == NPC)
            {
                return collection.Stats;
            }
        }

        return null;
    }

    public void UnlockNPCStat(string statID)
    {
        foreach (NPCStat stat in NPCStats)
        {
            if (stat.StatID == statID)
            {
                UnlockNPCStat(stat);
                return;
            }
        }

        Debug.Log($"Could not find NPC stat of ID {statID}");
    }

    public void UnlockNPCStat(NPCStat stat)
    {
        foreach (NPCStatCollection NPCstat in statCollections)
        {
            if (NPCstat.CollectionNPC == stat.StatNPC)
            {
                if (NPCstat.Stats.Contains(stat))
                {
                    return;
                }

                NPCstat.Stats.Add(stat);
                return;
            }
        }

        statCollections.Add(new NPCStatCollection(stat.StatNPC, new List<NPCStat>() { stat }));
    }
}

public class NPCStatCollection
{
    private NPCs NPC;
    private List<NPCStat> stats;

    public NPCs CollectionNPC { get { return NPC; } }
    public List<NPCStat> Stats { get { return stats; } }

    public NPCStatCollection(NPCs NPC, List<NPCStat> stats)
    {
        this.NPC = NPC;
        this.stats = stats;
    }
}
