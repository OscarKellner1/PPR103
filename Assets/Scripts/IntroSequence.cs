using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroSequence : MonoBehaviour
{
    [SerializeField]
    private GameObject uiPrefab;

    [SerializeField]
    private string[] textLines;

    private GameObject uiObject;
    private TMP_Text textBox;

    private int lineIndex;
    private InputAction nextLineAction;
    private bool isFinished;


    void Start()
    {
        AddUI();
        nextLineAction = InputUtility.Controls.Dialogue.NextLine;
        InputUtility.SetInputType(InputType.Dialogue);
    }

    private void Update()
    {
        if (nextLineAction.triggered)
        {
            if (lineIndex < textLines.Length - 1)
            {
                AdvanceText();
            }
            else if (!isFinished)
            {
                Finish();
            }
            else
            {
                return;
            }
        }
    }


    private void AddUI()
    {
        uiObject = UISystem.AddElement(uiPrefab);
        textBox = uiObject.GetComponentInChildren<TMP_Text>();
        
        textBox.text = (0 < textLines.Length) ? textLines[0] : "NO_TEXT_FOUND" ;
        lineIndex = 0;
    }

    private void AdvanceText()
    {
        lineIndex++;
        textBox.text = textLines[lineIndex];
    }

    private void Finish()
    {
        textBox.text = "";
        isFinished = true;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnDisable()
    {
        Destroy(uiObject);
    } 
}
