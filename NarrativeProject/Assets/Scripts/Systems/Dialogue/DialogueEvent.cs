using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Event")]
public class DialogueEvent : ScriptableObject
{
    public virtual void TriggerEvent()
    {
        // Create classes that inherit this class and override this method to add extra functionality
        // Ex. Conditional Events
    }
}