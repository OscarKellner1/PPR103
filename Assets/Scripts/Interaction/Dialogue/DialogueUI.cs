// ===========================
// DialogueUI.cs (Handles UI Animations and Visuals)
// Handles fade-ins, text scrolling, and UI transitions.
// ===========================
using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public TMP_Text dialogueText;
    public DialogueManager manager;
    public float textSpeed = 0.05f;

    public void DisplayText(string text)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(text));
    }

    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        manager.FinishedText();
    }
}
