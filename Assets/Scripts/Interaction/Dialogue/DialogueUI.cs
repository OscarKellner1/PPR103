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

    private void Start()
    {
 
    }
    public void DisplayText(string text,float speed)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(text, speed, manager));
    }

    IEnumerator TypeText(string text,float speed, DialogueManager man )
    {
        if(speed == 0)
        {
            dialogueText.text = text;
            
        }
        else
        {
            dialogueText.text = "";
            foreach (char c in text)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(speed);
            }
        }
        
        if (man != null)
        {
            man.FinishedText();
        }

           
        

       
    }
}
