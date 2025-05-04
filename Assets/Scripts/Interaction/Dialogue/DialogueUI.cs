using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public TMP_Text dialogueText;  // Reference to the TMP_Text component where the dialogue will be displayed
    public DialogueManager manager; // Reference to the DialogueManager
    public AudioSource audioSource; // Reference to AudioSource for playing sound

    // This method is called by the DialogueManager to start typing out the dialogue
    public void DisplayText(string text, float speed, AnimalSoundDictionary soundDictionary)
    {
        StopAllCoroutines();  // Stop any existing typing coroutine
        StartCoroutine(TypeText(text, speed, manager, soundDictionary));  // Start a new typing coroutine
    }

    // The TypeText coroutine that handles the typing animation of the text
    IEnumerator TypeText(string text, float speed, DialogueManager man, AnimalSoundDictionary soundDictionary)
    {
        if (speed <= 0f)
        {
            // Instant display
            dialogueText.text = text;
        }
        else
        {
            dialogueText.text = "";
            // Split into lines for the 0.5s line-break pause
            string[] lines = text.Split('\n');

            // MOVE wordCount OUTSIDE the line-loop so it accumulates globally
            int wordCount = 0;

            foreach (string line in lines)
            {
                // Split this line into words
                string[] words = line.Split(' ');

                foreach (string word in words)
                {
                    // —————— play on every 2nd word globally ——————
                    if (soundDictionary != null && wordCount % 2 == 0)
                    {
                        var clip = soundDictionary.GetRandomSound();
                        if (clip != null)
                            audioSource.PlayOneShot(clip);
                    }
                    wordCount++;

                    // TYPE OUT THE WORD…
                    foreach (char c in word)
                    {
                        dialogueText.text += c;
                        yield return new WaitForSeconds(speed);
                    }

                    // …then a space, plus a tiny pause
                    dialogueText.text += " ";
                    yield return new WaitForSeconds(speed * 0.1f);
                }

                // After each line, add newline + longer pause
                dialogueText.text += "\n";
                yield return new WaitForSeconds(0.5f);
            }
        }

        // Done typing!
        man?.FinishedText();
    }

}
