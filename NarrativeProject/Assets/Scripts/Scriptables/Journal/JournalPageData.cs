using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Journals/Journal Page Data")]
public class JournalPageData : ScriptableObject
{
    [SerializeField] private string journalDataID;
    [SerializeField] private string journalDataTitle;
    [SerializeField] [TextArea(5, 9)] private  string journalDataContent;

    public string JournalDataID { get { return journalDataID; } }
    public string JournalDataTitle { get {  return journalDataTitle; } }
    public string JournalDataContent { get {  return GetJournalDataContent(); } }

    public virtual string GetJournalDataContent()
    {
        return journalDataContent;
    }
}