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
    public GameObject PCan;
    public AnimalSoundDictionary Sounds;
    public AudioSource AudioSource;

    [System.Serializable]
    public class PosterEntry
    {
        [TextArea] public string text; // Multi-line text input
        public float typingSpeed = 0.05f;
        public bool newLine = true;
        public bool silent;
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

            yield return TypeText(entry.text, entry.typingSpeed, entry.silent);
        }
    }

    private IEnumerator TypeText(string text, float speed, bool silent)
    {

        if (speed == 0)
        {
            AudioClip randomSound = null;
            if (silent == false)
            {
                randomSound = Sounds.GetRandomSound();  // Get a random sound
            }

            
            if (randomSound != null)
            {
                AudioSource.PlayOneShot(randomSound);  // Play the sound
            }
            textDisplay.text += text;
        }
        else
        {
            foreach (char letter in text)
            {
                AudioClip randomSound = null;
                if (silent == false)
                {
                    randomSound = Sounds.GetRandomSound();  // Get a random sound
                }
                if (randomSound != null)
                {
                    AudioSource.PlayOneShot(randomSound);  // Play the sound
                }
                textDisplay.text += letter;
                
                yield return new WaitForSeconds(speed);
            }
        }
    }
    private void Start()
    {
        StartCoroutine(GetPcan());
    }
    private IEnumerator GetPcan()
    {
        yield return new WaitForSeconds(0.5f);
        PCan = GameObject.Find("PosterCanvas");
        videoPlayer = PCan.GetComponentInChildren<VideoPlayer>();
        uiAnimator = PCan.GetComponentInChildren<Animator>();
        textDisplay = PCan.GetComponentInChildren<TextMeshProUGUI>();
    }
}
