using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : Singleton<InteractableManager>
{
    [SerializeField] private GameObject interactablesBackground;
    [SerializeField] private GameObject pageHolder;
    [SerializeField] List<Interactable> interactables = new List<Interactable>();

    private Animator interactablesBackgroundAnimator;
    private Animator pageHolderAnimator;
    private bool displayingPage = false;
    private bool animatingPage = false;

    private GameObject page;

    protected override void Awake()
    {
        base.Awake();

        interactablesBackgroundAnimator = interactablesBackground.GetComponent<Animator>();
        pageHolderAnimator = pageHolder.GetComponent<Animator>();
    }

    public void DisplayInteractable(string interactableID)
    {
        if (displayingPage)
        {
            return;
        }

        foreach (var interactable in interactables)
        {
            if (interactableID == interactable.InteractableID)
            {
                if (interactable.UnlockEvent != null)
                {
                    interactable.UnlockEvent.TriggerEvent();
                }

                DisplayInteractablePage(interactable.InteractablePagePrefab);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !animatingPage)
        {
            DisplayInteractable("debug_interactable");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !animatingPage)
        {
            HideInteractablePage();
        }


    }

    public void DisplayInteractablePage(GameObject interactablePage)
    {
        page = Instantiate(interactablePage, pageHolder.transform);

        StartCoroutine(DisplayInteractablePageAnimation());
    }

    public void HideInteractablePage()
    {
        if (!displayingPage)
        {
            return;
        }

        StartCoroutine(HideInteractablePageAnimation());
    }

    public IEnumerator DisplayInteractablePageAnimation()
    {
        displayingPage = true;
        animatingPage = true;
        interactablesBackgroundAnimator.SetBool("Show", true);
        pageHolderAnimator.SetBool("Show", true);
        yield return new WaitForSeconds(1);
        animatingPage = false;
    }

    public IEnumerator HideInteractablePageAnimation()
    {
        displayingPage = false;
        animatingPage = true;
        interactablesBackgroundAnimator.SetBool("Show", false);
        pageHolderAnimator.SetBool("Show", false);
        yield return new WaitForSeconds(1);
        animatingPage = false;

        if (page != null)
        {
            Destroy(page);
        }
    }
}
