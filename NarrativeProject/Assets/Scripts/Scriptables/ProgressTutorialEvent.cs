using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Event/Progress Tutorial Event")]
public class ProgressTutorialEvent : DialogueEvent
{
    [SerializeField] private int tutorialStage = 0;

    public override void TriggerEvent()
    {
        TutorialManager.Instance.ProgressTutorial(tutorialStage);
    }
}
