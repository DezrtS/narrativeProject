using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Condition")]
public class DialogueCondition : ScriptableObject
{
    public virtual bool CanTrigger()
    {
        // Create classes that inherit this class and override this method to add extra functionality
        // Ex. Events that require specific calls
        return true;
    }
}