using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject timer;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timeLimit = 10;

    [SerializeField] private GameObject endScreen;
    private Animator endAnimator;

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

    public bool timerRunning;
    private float timerTime;

    protected override void Awake()
    {
        base.Awake();

        endScreen.SetActive(true);
        endAnimator = endScreen.GetComponent<Animator>();
        endScreen.SetActive(false);
    }

    public void StartTimer()
    {
        timer.SetActive(true);
        timerRunning = true;
        timerTime = timeLimit;
    }

    public void LoadNextPart()
    {
        TutorialManager.Instance.ProgressTutorial(3);
    }

    private void Update()
    {
        if (timerRunning)
        {
            timerTime -= Time.deltaTime;
            timerText.text = "Time Left: " + Mathf.Round(timerTime).ToString();

            if (timerTime <= 0)
            {
                timerText.text = "Time Left: 0";
                timerRunning = false;
                timer.SetActive(false);
                LoadNextPart();
                Debug.Log("Load Stuck in Room");
            }
        }
    }

    public void EndGame()
    {
        endScreen.SetActive(true);
        endAnimator.SetBool("Block", true);
        endAnimator.SetTrigger("Start");
    }


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