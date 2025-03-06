// ColorGateManager Script
// Created by: Connor
// Last edited: March 5, 2025 || 10:59pm
// Main function: Manages the generation of a random sequence of color materials, assigns them to objects, and manages the correct order for the ColorKey checks.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGateManager : MonoBehaviour
{
    public GameObject[] hintObjects; // The 4 objects that provide color hints (e.g., signs, background elements)
    public Material[] colorMaterials; // The 4 color materials (Red, Blue, Yellow, Green)

    public static List<Material> correctOrder = new List<Material>(); // List to hold the correct color order

    private void Start()
    {
        GenerateCombination();  // Call the function to generate the color combination on start
    }

    private void GenerateCombination()
    {
        // Step 1: Randomize the color list
        List<Material> randomizedColors = new List<Material>(colorMaterials);  // Copy the color materials to a new list
        ShuffleList(randomizedColors);  // Shuffle the color list randomly

        // Step 2: Randomize the hint objects
        List<GameObject> shuffledHintObjects = new List<GameObject>(hintObjects);  // Copy the hint objects to a new list
        ShuffleList(shuffledHintObjects);  // Shuffle the hint objects randomly

        // Step 3: Assign shuffled colors to hint objects' Renderer components
        for (int i = 0; i < shuffledHintObjects.Count; i++)
        {
            Renderer rend = shuffledHintObjects[i].GetComponent<Renderer>();  // Get the Renderer component of the hint object
            if (rend != null)  // Check if the Renderer exists
            {
                rend.material = randomizedColors[i];  // Assign the shuffled color to the object
            }
            else
            {
                Debug.LogWarning("No Renderer found on object " + shuffledHintObjects[i].name);  // Log a warning if no Renderer exists
            }
        }

        // Step 4: Set the correct order in the ColorKey script
        correctOrder = new List<Material>(randomizedColors);  // Set the correct color order list

        // Step 5: Debug log the shuffled hint objects and their colors for verification
        string orderMessage = "Hint Object Order (with colors): ";
        for (int i = 0; i < shuffledHintObjects.Count; i++)
        {
            orderMessage += shuffledHintObjects[i].name + " (" + randomizedColors[i].name + ")" + (i == shuffledHintObjects.Count - 1 ? "" : ", ");
        }
        Debug.Log(orderMessage);
    }

    // Utility method to shuffle a list (Fisher-Yates shuffle algorithm)
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);  // Randomly select an index from the list
            list[i] = list[randomIndex];  // Swap the current element with the randomly selected one
            list[randomIndex] = temp;  // Complete the swap
        }
    }
}
