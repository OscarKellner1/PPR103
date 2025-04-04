// ===========================
// DialogueManager.cs (Handles Dialogue Flow)
// Responsible for progressing dialogue, handling choices, and triggering events.
// ===========================
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using static DialogueData;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance; // Singleton for easy access

    [Header("UI Elements")]
    public GameObject dialogueBox;
    public GameObject speakerBox;
    public TMP_Text dialogueText;
    public TMP_Text speakerNameText;
    public Image portraitImage;

    [Header("Response UI")]
    public GameObject responseContainer;
    public Button responseButton1;
    public Button responseButton2;
    public TMP_Text responseButton1Text;
    public TMP_Text responseButton2Text;

    public List<CameraTarget> cameraTargets; // Add code to NPC on awake, to find Dialogue manager, find the entry with the same name as the NPC, and attach its transform to the transform parameter of the entry. Cinemachine!

    private DialogueData currentDialogue;
    private DialogueTrigger currentNPC;
    private int dialogueIndex = 0;
    private InputType previousInputType; //used to remember the input type used before entering dialogue
    public DialogueUI DUI;

    private void Awake()
    {
        
            Instance = this;
      
      
    }

    // Starts a new dialogue sequence
    public void StartDialogue(DialogueData dialogue, DialogueTrigger npc)
    {
        currentDialogue = dialogue;
        dialogueIndex = 0;
        currentNPC = npc; // Store reference to NPC

        // Start dialogue UI
        dialogueBox.SetActive(true);
        speakerBox.SetActive(true);
        previousInputType = InputUtility.InputType;
        InputUtility.SetInputType(InputType.Dialogue);

        ShowDialogueEntry();
    }

    // Handles the display of each dialogue entry in the sequence
    void ShowDialogueEntry()
    {
        currentNPC.isTalking = true;
        // If we have reached the end of the dialogue entries, end the conversation
        if (dialogueIndex >= currentDialogue.dialogueEntries.Count)
        {
            EndDialogue();
            return;
        }

        DialogueData.DialogueEntry entry = currentDialogue.dialogueEntries[dialogueIndex];

        // Check if the cameraContext matches an NPC/target name
      //  SwitchCamera(entry.cameraContext);

        // Check conditions before displaying dialogue entry
        if (!ConditionChecker.Instance.AreConditionsMet(entry.conditions))
        {
            // If conditions aren't met, end the current conversation and start a new one
            EndDialogue();
            StartDialogue(entry.failedDialogue, currentNPC); // Start a new conversation if conditions fail
            return;
        }

        speakerNameText.text = entry.speakerName;
        portraitImage.sprite = entry.portrait;
        DUI.DisplayText(entry.dialogueText);
       // dialogueText.text = entry.dialogueText;

        // Handle auto-progress if required
        if (entry.autoProgress)
        {
            StartCoroutine(AutoProgressDialogue(entry.autoProgressDelay));
        }

        // Trigger any events tied to the dialogue entry
        entry.onDialogueEvent?.Invoke();
        ShowResponses(entry.responses);
    }

    void SwitchCamera(string cameraContext)
    {
        CameraTarget target = cameraTargets.Find(t => t.name == cameraContext);
        if (target != null)
        {
            CinemachineManager.Instance.SwitchCamera(target.cameraTransform);
        }
        else
        {
            Debug.Log("Bro No Cam");
        }
    }

    // Automatically progresses to the next dialogue entry after a delay
    IEnumerator AutoProgressDialogue(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextDialogueEntry();
    }

    // Proceed to the next dialogue entry in the sequence
    public void NextDialogueEntry()
    {
        dialogueIndex++;
        ShowDialogueEntry();
    }

    // Display responses and handle response buttons
    public void ShowResponses(List<DialogueData.PlayerResponse> responses)
    {
        responseContainer.SetActive(responses.Count > 0); // Show response container if responses exist
        responseButton1.gameObject.SetActive(false);
        responseButton2.gameObject.SetActive(false);

        // Handle the first response button
        if (responses.Count > 0)
        {
            responseButton1.gameObject.SetActive(true);
            responseButton1Text.text = responses[0].responseText;
            responseButton1.onClick.RemoveAllListeners();
            responseButton1.onClick.AddListener(() => SelectResponse(responses[0]));
        }

        // Handle the second response button if it exists
        if (responses.Count > 1)
        {
            responseButton2.gameObject.SetActive(true);
            responseButton2Text.text = responses[1].responseText;
            responseButton2.onClick.RemoveAllListeners();
            responseButton2.onClick.AddListener(() => SelectResponse(responses[1]));
        }
    }

    // Handle the player's response selection
    public void SelectResponse(DialogueData.PlayerResponse response)
    {
        // If the response has the "Ender" flag set to true, end the conversation
        if (response.Ender)
        {
            EndDialogue();
        }
        else if (response.nextDialogue != null)
        {
            // If there's a next dialogue to go to, start it (branching)
            StartDialogue(response.nextDialogue, currentNPC); // Trigger the new conversation based on the response
        }
        else
        {
            // Otherwise, continue to the next entry in the current dialogue
            NextDialogueEntry();
        }
    }

    // End the current conversation
    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        speakerBox.SetActive(false);
        responseButton1.gameObject.SetActive(false);
        responseButton2.gameObject.SetActive(false);

        // Stop NPC from talking
        if (currentNPC != null)
        {
            currentNPC.StopTalking();
            currentNPC = null;
        }

        if (currentDialogue?.AfterDialogueEvent != null)
        {
            currentDialogue.AfterDialogueEvent.Invoke();
        }

        InputUtility.SetInputType(previousInputType);
    }
    public void FinishedText()
    {
        currentNPC.StopTalking();
    }

}
