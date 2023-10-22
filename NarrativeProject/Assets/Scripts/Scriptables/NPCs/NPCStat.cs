using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/NPCs/NPC Stat")]
public class NPCStat : ScriptableObject
{
    [SerializeField] private NPCs statNPC;
    [SerializeField] private string statID;
    [SerializeField][TextArea(3, 9)] private string statContent;

    public NPCs StatNPC { get { return statNPC; } }
    public string StatID { get { return statID; } }
    public string StatContent { get { return statContent; } }
}