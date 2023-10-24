using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Event/End Game Event")]
public class EndGameEvent : DialogueEvent
{

    public override void TriggerEvent()
    {
        GameManager.Instance.EndGame();
    }
}

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Event/Reset Scene Event")]
public class ResetSceneEvent : DialogueEvent
{

    public override void TriggerEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
