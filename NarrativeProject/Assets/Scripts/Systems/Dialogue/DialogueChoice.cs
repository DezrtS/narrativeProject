using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoice : MonoBehaviour
{
    private Choice choice;

    public Choice Choice { get { return choice; } set { choice = value; } }

    public void ActivateChoice()
    {
        DialogueManager.Instance.ActivateChoice(choice);
    }
}
