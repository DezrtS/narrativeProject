using UnityEngine;

public enum Condition
{
    None,

    AskAnnieKillerOpinion,
    AskBradKillerOpinion,
    AskFlanKillerOpinion,
    AskLucyKillerOpinion,
    AskSteveKillerOpinion,

    AskAnnieAboutCase,
    AskBradAboutCase,
    AskFlanAboutCase,
    AskLucyAboutCase,
    AskSteveAboutCase,

    AskFlanAboutRats,
    AskLucyAboutBathroom,
    AskSteveAboutArgument
}

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Condition")]
public class DialogueCondition : ScriptableObject
{
    [SerializeField] private Condition condition = Condition.None;

    public virtual bool CanTrigger()
    {
        switch (condition)
        {
            case Condition.None:
                return true;
            case Condition.AskAnnieKillerOpinion:
                return GameManager.Instance.canAskAnnieKillerOpinion;
            case Condition.AskBradKillerOpinion:
                return GameManager.Instance.canAskBradKillerOpinion;
            case Condition.AskFlanKillerOpinion:
                return GameManager.Instance.canAskFlanKillerOpinion;
            case Condition.AskLucyKillerOpinion:
                return GameManager.Instance.canAskLucyKillerOpinion;
            case Condition.AskSteveKillerOpinion:
                return GameManager.Instance.canAskSteveKillerOpinion;
            case Condition.AskAnnieAboutCase:
                return GameManager.Instance.canAskAnnieAboutCase;
            case Condition.AskBradAboutCase:
                return GameManager.Instance.canAskBradAboutCase;
            case Condition.AskFlanAboutCase:
                return GameManager.Instance.canAskFlanAboutCase;
            case Condition.AskLucyAboutCase:
                return GameManager.Instance.canAskLucyAboutCase;
            case Condition.AskSteveAboutCase:
                return GameManager.Instance.canAskSteveAboutCase;
            case Condition.AskFlanAboutRats:
                return GameManager.Instance.canAskFlanAboutRats;
            case Condition.AskLucyAboutBathroom:
                return GameManager.Instance.canAskLucyAboutBathroom;
            case Condition.AskSteveAboutArgument:
                return GameManager.Instance.canAskSteveAboutArgument;
            default:
                Debug.Log("Condition Error");
            return false;
        }
    }
}