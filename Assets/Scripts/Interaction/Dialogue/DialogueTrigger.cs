using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class Conversation
    {
        public string conversationName; // Name in the Inspector for organization
        public DialogueData dialogue; // The actual dialogue
        public List<DialogueCondition> conditions; // Conditions for this conversation
    }

    [Header("NPC Conversations (Ordered by Importance)")]
    public List<Conversation> conversations = new List<Conversation>(); // List of possible conversations

    private void OnMouseDown()
    {
        TriggerDialogue();
    }
    public void TriggerDialogue()
    {
        foreach (var conversation in conversations)
        {
            if (ConditionChecker.Instance.AreConditionsMet(conversation.conditions))
            {
                DialogueManager.Instance.StartDialogue(conversation.dialogue);
                return; // Stop after finding the first valid conversation
            }
        }

        Debug.Log("No valid conversations found for this NPC.");
    }
}