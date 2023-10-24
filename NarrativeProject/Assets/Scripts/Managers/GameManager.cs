using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool canAskAnnieKillerOpinion;
    public bool canAskBradKillerOpinion;
    public bool canAskFlanKillerOpinion;
    public bool canAskLucyKillerOpinion;
    public bool canAskSteveKillerOpinion;

    public bool canAskAnnieAboutCase;
    public bool canAskBradAboutCase;
    public bool canAskFlanAboutCase;
    public bool canAskLucyAboutCase;
    public bool canAskSteveAboutCase;

    public bool canAskFlanAboutRats;
    public bool canAskLucyAboutBathroom;
    public bool canAskSteveAboutArgument;

    public void SetCondition(Condition condition, bool conditionState)
    {
        switch (condition)
        {
            case Condition.None:
                return;
            case Condition.AskAnnieKillerOpinion:
                canAskAnnieKillerOpinion = conditionState;
                return;
            case Condition.AskBradKillerOpinion:
                canAskBradKillerOpinion = conditionState;
                return;
            case Condition.AskFlanKillerOpinion:
                canAskFlanKillerOpinion = conditionState;
                return;
            case Condition.AskLucyKillerOpinion:
                canAskLucyKillerOpinion = conditionState;
                return;
            case Condition.AskSteveKillerOpinion:
                canAskSteveKillerOpinion = conditionState;
                return;
            case Condition.AskAnnieAboutCase:
                canAskAnnieAboutCase = conditionState;
                return;
            case Condition.AskBradAboutCase:
                canAskBradAboutCase = conditionState;
                return;
            case Condition.AskFlanAboutCase:
                canAskFlanAboutCase = conditionState;
                return;
            case Condition.AskLucyAboutCase:
                canAskLucyAboutCase = conditionState;
                return;
            case Condition.AskSteveAboutCase:
                canAskSteveAboutCase = conditionState;
                return;
            case Condition.AskFlanAboutRats:
                canAskFlanAboutRats = conditionState;
                return;
            case Condition.AskLucyAboutBathroom:
                canAskLucyAboutBathroom = conditionState;
                return;
            case Condition.AskSteveAboutArgument:
                canAskSteveAboutArgument = conditionState;
                return;
            default:
                Debug.Log("Set Condition Error");
                return;
        }
    }
}