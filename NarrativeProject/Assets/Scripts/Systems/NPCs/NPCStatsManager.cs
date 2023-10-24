using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCs
{
    Annie,
    Brad,
    Elon,
    Flan,
    Lucy,
    Steve,
    YourPartner,
    You,
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

                AudioManager.Instance.PlayOneShot(FMODEventManager.Instance.Writing.EventReference, Vector3.zero);

                NPCstat.Stats.Add(stat);
                return;
            } 
        }

        AudioManager.Instance.PlayOneShot(FMODEventManager.Instance.Writing.EventReference, Vector3.zero);
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
