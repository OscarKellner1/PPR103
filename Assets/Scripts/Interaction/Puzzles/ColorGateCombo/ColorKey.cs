// ColorKey Script
// Created by: Connor
// Last edited: March 5, 2025 || 10:57pm
// Main function: Handles interactions with ColorKey objects, checks the order of key presses, and updates materials based on success, failure, or lockout.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorKey : MonoBehaviour
{
    public Material normalMaterial;  // Default material (normal state of the key)
    public Material highlightMaterial;  // Material when the key is pressed (highlighted)
    public Material successMaterial;  // Material for success (green glow when correct)
    public Material failMaterial;  // Material for failure (red glow when wrong)
    public Material lockedMaterial;  // Material when locked out (darkened)

    public AudioClip interactSound;  // Sound when the key is pressed
    public AudioClip successSound;  // Sound when the correct sequence is achieved
    public AudioClip failSound;  // Sound when the sequence fails
    public AudioClip lockoutSound;  // Sound when the keys are locked out after too many failures

    private static List<ColorKey> pressedKeys = new List<ColorKey>();  // List to keep track of pressed keys
    private static int failCount = 0;  // Counter to track failures
    private static bool isChecking = false;  // Flag to prevent multiple sequence checks at the same time
    private static bool isLocked = false;  // Flag to track if the keys are locked

    public Renderer rend;  // Renderer to change material on the key
    private AudioSource audioSource;  // AudioSource to play sounds

    private void Start()
    {
        rend = GetComponent<Renderer>();  // Initialize the renderer component
        audioSource = gameObject.AddComponent<AudioSource>();  // Add an AudioSource to the gameObject
        rend.material = normalMaterial;  // Set the initial material to normal
    }

    public void GotPressed()
    {
        // If the keys are locked, the sequence is being checked, or this key has already been pressed, do nothing
        if (isLocked || isChecking || pressedKeys.Contains(this)) return;

        pressedKeys.Add(this);  // Add the current key to the pressed keys list
        rend.material = highlightMaterial;  // Change the material to the highlight material
        PlaySound(interactSound);  // Play the interaction sound

        // If 4 keys have been pressed, start checking the sequence
        if (pressedKeys.Count == 4)
        {
            StartCoroutine(CheckSequence());  // Start the sequence check coroutine
        }
    }

    private IEnumerator CheckSequence()
    {
        isChecking = true;  // Set checking flag to true
        yield return new WaitForSeconds(1f);  // Wait for 1 second before checking the sequence

        // Check if the pressed keys match the correct order
        if (CheckCorrectOrder())
        {
            PlaySound(successSound);  // Play the success sound
            SetAllMaterials(successMaterial);  // Set all pressed keys to success material
            PlayGateAnimation();  // Trigger gate animation (you need to implement this)
        }
        else
        {
            failCount++;  // Increment the fail count
            PlaySound(failSound);  // Play the failure sound
            SetAllMaterials(failMaterial);  // Set all pressed keys to failure material

            // If there have been 3 or more failures, lock the keys
            if (failCount >= 3)
            {
                isLocked = true;  // Lock the keys
                PlaySound(lockoutSound);  // Play the lockout sound
                yield return new WaitForSeconds(1f);  // Wait for 1 second
                SetAllMaterials(lockedMaterial);  // Set all keys to locked material
            }
            else
            {
                yield return new WaitForSeconds(1f);  // Wait for 1 second before resetting
                ResetKeys();  // Reset all keys to normal state
            }
        }

        isChecking = false;  // Set checking flag to false
    }

    private bool CheckCorrectOrder()
    {
        // Debugging logs for pressed order
        string pressedOrder = "Pressed Order: ";
        foreach (ColorKey key in pressedKeys)
        {
            pressedOrder += key.GetComponent<ColorKey>().normalMaterial + " ";  // Display the key's normal material for debugging
        }
        Debug.Log(pressedOrder);

        // Debugging logs for correct order
        string correctOrderStr = "Correct Order: ";
        foreach (Material key in ColorGateManager.correctOrder)
        {
            correctOrderStr += key.name + " ";  // Display the correct material for debugging
        }
        Debug.Log(correctOrderStr);

        // Check if the pressed keys' normal materials match the correct order of normal materials
        for (int i = 0; i < pressedKeys.Count; i++)
        {
            if (pressedKeys[i].normalMaterial != ColorGateManager.correctOrder[i])
            {
                return false;  // Return false if the order doesn't match
            }
        }
        return true;  // Return true if the order matches
    }

    private void ResetKeys()
    {
        // Reset the material of all pressed keys back to their normal state
        foreach (ColorKey key in pressedKeys)
        {
            key.rend.material = key.normalMaterial;
        }
        pressedKeys.Clear();  // Clear the pressed keys list
    }

    private void SetAllMaterials(Material mat)
    {
        // Set all pressed keys to the same material
        foreach (ColorKey key in pressedKeys)
        {
            key.rend.material = mat;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        // Play the provided sound clip
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayGateAnimation()
    {
        // Debug log for the gate opening animation (implement animation trigger here)
        Debug.Log("Gate Opens!");
    }
}
