using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private GameObject screenBlocker;

    [SerializeField] private GameObject partner;

    [SerializeField] private Vector3 firstPartnerLocation;
    [SerializeField] private Vector3 secondPartnerLocation;
    [SerializeField] private DialogueSequence firstDialogueSequence;
    [SerializeField] private DialogueSequence secondDialogueSequence;
    //[SerializeField] private GameObject firstInvisibleWall;
    [SerializeField] private GameObject secondInvisibleWall;

    [SerializeField] private Vector3 endPosition;

    private Animator screenBlockerAnimator;
    private InteractableObject partnerInteractable;

    public bool isBlocking;

    protected override void Awake()
    {
        base.Awake();

        screenBlocker.SetActive(true);
        screenBlockerAnimator = screenBlocker.GetComponent<Animator>();
        screenBlockerAnimator.SetBool("Block", false);
        screenBlockerAnimator.SetTrigger("Start");

        partnerInteractable = partner.GetComponent<InteractableObject>();

        isBlocking = true;

        StartCoroutine(SetActiveFalse());
    }

    private void Start()
    {
        DialogueManager.Instance.StartDialogueSequence(DialogueSequenceManager.Instance.GetSequence("start"));
    }

    public void TriggerBlock(bool block)
    {
        screenBlocker.SetActive(true);
        screenBlockerAnimator.SetBool("Block", block);
        screenBlockerAnimator.SetTrigger("Start");

        isBlocking = true;

        if (!block)
        {
            StartCoroutine(SetActiveFalse());
        }
    }

    public void ProgressTutorial(int stage)
    {
        switch (stage)
        {
            case 0:
                return;
            case 1:
                StartCoroutine(ProgressTutorialStage(firstPartnerLocation));
                partnerInteractable.SetDialogueSequence(firstDialogueSequence);
                //firstInvisibleWall.SetActive(false);
                return;
            case 2:
                StartCoroutine(ProgressTutorialStage(secondPartnerLocation));
                partnerInteractable.SetDialogueSequence(secondDialogueSequence);
                secondInvisibleWall.SetActive(false);
                GameManager.Instance.StartTimer();
                return;
            case 3:
                StartCoroutine(ProgressEnding());
                return;
            default:
                return;
        }
    }

    public IEnumerator SetActiveFalse()
    {
        Debug.Log("Disabling Fade");
        yield return new WaitForSeconds(1.5f);
        screenBlocker.SetActive(false);
        isBlocking = false;
    }

    public IEnumerator ProgressTutorialStage(Vector3 position)
    {
        TriggerBlock(true);
        PlayerController.Instance.transitionMode = true;

        yield return new WaitForSeconds(2f);

        PlayerController.Instance.transitionMode = false;

        partner.transform.position = position;
        TriggerBlock(false);
    }

    public IEnumerator ProgressEnding()
    {
        TriggerBlock(true);

        yield return new WaitForSeconds(2f);

        CharacterController characterController = PlayerController.Instance.gameObject.GetComponent<CharacterController>();
        characterController.enabled = false;
        PlayerController.Instance.transform.position = endPosition;
        characterController.enabled = true;

        TriggerBlock(false);
    }
}
