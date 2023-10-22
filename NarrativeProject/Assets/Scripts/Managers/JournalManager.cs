using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalManager : Singleton<JournalManager>
{
    [SerializeField] private float pageTurnTime = 1;

    [SerializeField] private GameObject journalGameobject;

    [SerializeField] private JournalPage leftPage;  
    [SerializeField] private JournalPage rightPage;  
    [SerializeField] private JournalPage turningPage;

    [SerializeField] private List<JournalPageData> pageDatas = new List<JournalPageData>();

    private bool isJournalActive = false;
    private bool isTurning;
    private bool isTurningToPage = false;
    private int currentJournalPage = 0;
    private int maxPages = 25;

    private void Start()
    {
        //DisplayJournal();


    }



    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && !isTurningToPage)
        {
            TurnPage(false, 1);
        } 
        else if (Input.GetKey(KeyCode.D) && !isTurningToPage)
        {
            TurnPage(true, 1);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!isJournalActive)
            {
                DisplayJournal();
            } 
            else
            {
                HideJournal();
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            NPCStatsManager.Instance.UnlockNPCStat("initial_clue");
        }
    }

    public void DisplayJournal()
    {
        journalGameobject.SetActive(true);
        isJournalActive = true;
        PlayerController.Instance.journalMode = isJournalActive;

        LoadPageEntry(currentJournalPage, leftPage);
        LoadPageEntry(currentJournalPage + 1, rightPage);
    }

    public void HideJournal()
    {
        StopAllCoroutines();
        journalGameobject.SetActive(false);
        isJournalActive = false;
        PlayerController.Instance.journalMode = isJournalActive;
        isTurning = false;
        isTurningToPage = false;
        turningPage.DisablePage();
    }

    public void TurnPage(bool forwards, float timeMultiplier)
    {
        if (isTurning || !isJournalActive || ((currentJournalPage == 0 && !forwards) || (currentJournalPage >= maxPages && forwards)))
        {
            return;
        }

        isTurning = true;

        int changingPage = forwards ? 1 : 0;

        turningPage.SwitchPageSide(forwards);
        turningPage.EnablePage();
        LoadPageEntry(currentJournalPage + changingPage, turningPage);

        currentJournalPage += (forwards ? 2 : -2);

        LoadPageEntry(currentJournalPage + changingPage, forwards ? rightPage : leftPage);

        StartCoroutine(AnimateTurnPage(forwards, timeMultiplier));
    }

    public void TurnToPage(int pageNumber, float timeMultiplier)
    {
        if (pageNumber == currentJournalPage || isTurningToPage)
        {
            return;
        }

        StartCoroutine(AnimateTurnToPage(pageNumber, timeMultiplier));
    }

    private IEnumerator AnimateTurnPage(bool forwards, float timeMultiplier)
    {
        AudioManager.Instance.PlayOneShot(FMODEventManager.Instance.PageTurn.EventReference, Vector3.zero);

        float turnTime = pageTurnTime / timeMultiplier * 0.5f;
        float startingTime = Time.timeSinceLevelLoad;

        while (turningPage.GetPageScale() != 0 && Time.timeSinceLevelLoad - startingTime < turnTime)
        {
            yield return null;
            turningPage.ScalePage(1 - SmoothValue(startingTime, turnTime));
        }

        turningPage.ScalePage(0);
        turningPage.SwitchPageSide(!forwards);
        LoadPageEntry(currentJournalPage + (!forwards ? 1 : 0), turningPage);

        startingTime = Time.timeSinceLevelLoad;

        while (turningPage.GetPageScale() != 1 && Time.timeSinceLevelLoad - startingTime < turnTime)
        {
            yield return null;
            turningPage.ScalePage(SmoothValue(startingTime, turnTime));
        }

        turningPage.ScalePage(1);
        turningPage.DisablePage();
        LoadPageEntry(currentJournalPage + (!forwards ? 1 : 0), !forwards ? rightPage : leftPage);
        isTurning = false;
    }

    private IEnumerator AnimateTurnToPage(int pageNumber, float timeMultiplier)
    {
        bool forwards;

        if (pageNumber > currentJournalPage)
        {
            forwards = true;
        } 
        else
        {
            forwards = false;
        }

        if (!((currentJournalPage == 0 && !forwards) || (currentJournalPage >= maxPages && forwards)))
        {
            isTurningToPage = true;

            while (currentJournalPage != pageNumber && currentJournalPage != pageNumber - 1)
            {
                yield return null;

                if (!isTurning)
                {
                    TurnPage(forwards, timeMultiplier);
                }
            }

            isTurningToPage = false;
        }
    }

    public void LoadPageEntry(int pageDataIndex, JournalPage journalPage)
    {
        if (pageDataIndex >= pageDatas.Count)
        {
            journalPage.LoadJournalPageContent("", "", pageDataIndex.ToString());
            return;
        }

        JournalPageData pageData = pageDatas[pageDataIndex];

        if (pageData == null)
        {
            journalPage.LoadJournalPageContent("", "", pageDataIndex.ToString());
            return;
        }

        journalPage.LoadJournalPageContent(pageData.JournalDataTitle, pageData.JournalDataContent, pageDataIndex.ToString());
    }

    private float SmoothValue(float startingTime, float timeTillCompletion)
    {
        return (0.5f * Mathf.Cos(((Time.timeSinceLevelLoad - startingTime - timeTillCompletion) * Mathf.PI) / timeTillCompletion) + 0.5f);
    }

}
