using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueTrigger;

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class Conversation
    {
        public string conversationName; // Name in the Inspector for organization
        public DialogueData dialogue; // The actual dialogue
        public List<DialogueCondition> conditions; // Conditions for this conversation
    }

    public GameObject Me;
    public string npcName; // Set this in the inspector or dynamically
    public bool isTalking = false; // True when NPC is talking
    public bool AlternateSprites = false;

    private Renderer npcRenderer;
    private int matIndex = 1; // Swaps between 1 and 2
    private Dictionary<string, Material> materialDict = new Dictionary<string, Material>();
    

    public List<Material> materials; // Assign materials in the inspector (must be 4: idle1, idle2, talk1, talk2)

    [Header("NPC Conversations (Ordered by Importance)")]
    public List<Conversation> conversations = new List<Conversation>(); // List of possible conversations

    private void Start()
    {
        Me = this.gameObject;
        npcRenderer = GetComponent<Renderer>();

        // Ensure we have exactly 4 materials
        if (materials.Count < 4)
        {
            Debug.LogError("Not enough materials assigned! Needs exactly 4.");
            return;
        }

        // Populate material dictionary
        materialDict.Add(npcName + "_False_1", materials[0]); // Idle 1
        materialDict.Add(npcName + "_False_2", materials[1]); // Idle 2
        materialDict.Add(npcName + "_True_1", materials[2]);  // Talking 1
        materialDict.Add(npcName + "_True_2", materials[3]);  // Talking 2
        materialDict.Add(npcName + "_False_1Alt", materials[4]); // Idle 1
        materialDict.Add(npcName + "_False_2Alt", materials[5]); // Idle 2
        materialDict.Add(npcName + "_True_1Alt", materials[6]);  // Talking 1
        materialDict.Add(npcName + "_True_2Alt", materials[7]);  // Talking 2


        StartCoroutine(MaterialSwapRoutine());
    }



    public void TriggerDialogue()
    {
        foreach (var conversation in conversations)
        {
            if (ConditionChecker.Instance.AreConditionsMet(conversation.conditions))
            {
                isTalking = true; // Set Talking to true when starting a conversation
                DialogueManager.Instance.StartDialogue(conversation.dialogue, this);
                return;
            }
        }

        Debug.Log("No valid conversations found for this NPC.");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TriggerDialogue();
        }
    }
    
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    public void StopTalking()
    {
        isTalking = false; // Allow external calls to stop talking
    }

    private IEnumerator MaterialSwapRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            // Swap between 1 and 2
            matIndex = (matIndex == 1) ? 2 : 1;

            // Generate the key
            string code = npcName + "_" + isTalking + "_" + matIndex;
            if(AlternateSprites)
            {
                code = code + "Alt";
            }
            // Apply the correct material
            if (materialDict.TryGetValue(code, out Material newMaterial))
            {
                npcRenderer.material = newMaterial;
            }
            else
            {
                Debug.LogError("Material not found for: " + code);
            }
        }
    }
}
