using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Interactable/Interactable Page")]
public class Interactable : ScriptableObject
{
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject interactablePagePrefab;
    [SerializeField] private UnlockEvent unlockEvent;

    public string InteractableID {  get { return interactableID; } }
    public GameObject InteractablePagePrefab { get {  return interactablePagePrefab; } }
    public UnlockEvent UnlockEvent { get {  return unlockEvent; } }
}