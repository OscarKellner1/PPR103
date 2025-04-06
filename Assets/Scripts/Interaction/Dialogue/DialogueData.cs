// ===========================
// DialogueData.cs (Scriptable Object)
// Stores dialogue entries, including text, choices, and events.
// ===========================
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string speakerName; // Name of the speaker
        public Sprite portrait; // Speaker portrait image
        public string cameraContext;  // The name of the object to focus the camera on, a list of names and pairing game objects will be searched to find this.
        [TextArea(3, 5)] public string dialogueText; // The actual dialogue line
        public bool isThinking; // Whether it's a "thinking" dialogue
        public bool isSilent;
        [Tooltip("Default is 0.05f")]
        public float textspeed = 0.05f;
        public float waitTime; // Time to wait before showing this dialogue
        public bool autoProgress; // If true, auto-continues after delay
        public float autoProgressDelay; // Delay before auto-progressing

        public AudioClip voiceClip; // Voice clip for the dialogue
        public UnityEngine.Events.UnityEvent onDialogueEvent; // Events triggered in this entry

       /* public List<DialogueCondition> conditions; // Conditions required for this dialogue
        public DialogueData failedDialogue;*/
        public List<PlayerResponse> responses; // Player response options
    }

    [System.Serializable]
    public class PlayerResponse
    {
        public string responseText; // Text of the response
        public DialogueData nextDialogue; // The dialogue that follows this choice
        public bool Ender;
    }

    public List<DialogueEntry> dialogueEntries; // All entries in this dialogue}
    [Header("Events")]
    public UnityEngine.Events.UnityEvent AfterDialogueEvent = new UnityEngine.Events.UnityEvent();

    [System.Serializable]
    public class CameraTarget
    {
        public string name; // The name of the target to match with cameraContext
        public Transform cameraTransform; // The transform of the object to look at
    }
}
