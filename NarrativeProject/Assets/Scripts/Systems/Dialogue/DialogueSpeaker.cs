using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Speaker")]
public class DialogueSpeaker : ScriptableObject
{
    [SerializeField] private string speakerName;
    [SerializeField] private Color primarySpeakerColor;
    [SerializeField] private Color secondarySpeakerColor;

    public string SpeakerName { get { return speakerName; } }
    public Color PrimarySpeakerColor { get { return primarySpeakerColor; } }
    public Color SecondarySpeakerColor { get { return secondarySpeakerColor; } }
}