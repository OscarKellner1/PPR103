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
        if (speed == 0)
        {
            dialogueText.text = text;  // Display the full text immediately
        }
        else
        {
            dialogueText.text = "";  // Clear the text field before starting

            // Split the text into lines by newlines (\n)
            string[] lines = text.Split(new[] { '\n' });

            foreach (string line in lines)
            {
                // Split the line into words
                string[] words = line.Split(' ');

                foreach (string word in words)
                {
                    // Play a random sound at the start of each word using the AnimalSoundDictionary
                    if (soundDictionary != null)
                    {
                        AudioClip randomSound = soundDictionary.GetRandomSound();  // Get a random sound
                        if (randomSound != null)
                        {
                            audioSource.PlayOneShot(randomSound);  // Play the sound
                        }
                    }

                    // Type the word with the specified speed
                    foreach (char c in word)
                    {
                        dialogueText.text += c;  // Append each character to the dialogueText
                        yield return new WaitForSeconds(speed);  // Wait for the specified speed before typing the next character
                    }

                    // Add a space after the word
                    dialogueText.text += " ";
                    yield return null;  // Allows small delay between words
                }
                dialogueText.text += "\n";
                // After finishing one line, add a short delay (for the line break)
                yield return new WaitForSeconds(0.5f);  // 0.5 seconds delay after each line (you can adjust this time)
            }
        }

        // Once the text typing is complete, call FinishedText on the DialogueManager
        if (man != null)
        {
            man.FinishedText();  // Notify the DialogueManager that typing is complete
        }
    }
}
