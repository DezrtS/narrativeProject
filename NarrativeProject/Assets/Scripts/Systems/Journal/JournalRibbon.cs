using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalRibbon : MonoBehaviour
{
    [SerializeField] private int ribbonPage;
    [SerializeField] private float timeMultiplier = 1f;

    public void TurnToRibbonPage()
    {
        JournalManager.Instance.TurnToPage(ribbonPage, timeMultiplier);
    }
}