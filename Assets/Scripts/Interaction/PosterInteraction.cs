using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class PosterInteraction : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public TextMeshProUGUI textDisplay;
    public Animator uiAnimator; // Reference to Animator
    public VideoClip clip;
    private bool isInteracting = false;

    [System.Serializable]
    public class PosterEntry
    {
        [TextArea] public string text; // Multi-line text input
        public float typingSpeed = 0.05f;
        public bool newLine = true;
        public int LineBreaks = 1; // Number of line breaks if newLine is true
        public float SecondsBeforeThisShows; // Time to wait before starting this entry
    }

    [Header("Poster Entries (Editable in Inspector)")]
    public List<PosterEntry> entries = new List<PosterEntry>();

    public float disappearDelay = 3f; // Time to wait before transitioning to State 2
    private InputType previousInputType; // Used to remember the input type before entering dialogue

    public void InteractWithPoster()
    {
        if (!isInteracting)
        {
            previousInputType = InputUtility.InputType;

            InputUtility.SetInputType(InputType.Dialogue);
            Cursor.visible = false;
            StartCoroutine(ShowPosterSequence());
        }
    }

    private IEnumerator ShowPosterSequence()
    {
        isInteracting = true;
        textDisplay.text = "";

        // Step 1: Set Animator "State" to 1 (Start Appear animation)
        uiAnimator.SetFloat("State", 1);

        // Step 2: Wait a little before playing video (optional)
        yield return new WaitForSeconds(0.5f);
        videoPlayer.clip = clip;
        videoPlayer.Play();
        

        // Step 3: Type text dynamically
        yield return StartTyping(entries);

        // Step 4: Wait before triggering disappear animation
        yield return new WaitForSeconds(disappearDelay);
        uiAnimator.SetFloat("State", 2);

        isInteracting = false;

        yield return new WaitForSeconds(2);
        InputUtility.SetInputType(previousInputType);
        Destroy(gameObject);
    }

    private IEnumerator StartTyping(List<PosterEntry> entries)
    {
        foreach (var entry in entries)
        {
            yield return new WaitForSeconds(entry.SecondsBeforeThisShows); // Wait before typing

            if (entry.newLine)
            {
                textDisplay.text += new string('\n', entry.LineBreaks); // Add multiple line breaks
            }

            yield return TypeText(entry.text, entry.typingSpeed);
        }
    }

    private IEnumerator TypeText(string text, float speed)
    {
        if (speed == 0)
        {
            textDisplay.text += text;
        }
        else
        {
            foreach (char letter in text)
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(speed);
            }
        }
    }
}
