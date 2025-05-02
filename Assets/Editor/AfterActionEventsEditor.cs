using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CustomEditor(typeof(AfterActionEvents))]
public class AfterActionEventsEditor : Editor
{
    SerializedProperty actionMethods;
    SerializedProperty HOH;

    List<bool> methodFoldouts = new List<bool>();
    List<List<bool>> stepFoldouts = new List<List<bool>>();
    private bool showActionMethods = true;

    private void OnEnable()
    {
        actionMethods = serializedObject.FindProperty("actionMethods");
        HOH = serializedObject.FindProperty("HOH");
        SyncFoldouts();
    }

    private void SyncFoldouts()
    {
        List<bool> oldMethodFoldouts = new List<bool>(methodFoldouts);
        List<List<bool>> oldStepFoldouts = new List<List<bool>>(stepFoldouts);

        methodFoldouts = new List<bool>();
        stepFoldouts = new List<List<bool>>();

        for (int i = 0; i < actionMethods.arraySize; i++)
        {
            methodFoldouts.Add(i < oldMethodFoldouts.Count ? oldMethodFoldouts[i] : false);
            SerializedProperty instructions = actionMethods.GetArrayElementAtIndex(i).FindPropertyRelative("instructions");
            List<bool> steps = new List<bool>();
            for (int j = 0; j < instructions.arraySize; j++)
            {
                if (i < oldStepFoldouts.Count && j < oldStepFoldouts[i].Count)
                    steps.Add(oldStepFoldouts[i][j]);
                else
                    steps.Add(false);
            }
            stepFoldouts.Add(steps);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (HOH != null)
        {
            EditorGUILayout.PropertyField(HOH);
        }

        if (actionMethods == null)
            return;

        showActionMethods = EditorGUILayout.Foldout(showActionMethods, "Action Methods", true);

        if (showActionMethods)
        {
            for (int i = 0; i < actionMethods.arraySize; i++)
            {
                SerializedProperty actionMethod = actionMethods.GetArrayElementAtIndex(i);
                SerializedProperty methodName = actionMethod.FindPropertyRelative("methodName");
                SerializedProperty instructions = actionMethod.FindPropertyRelative("instructions");

                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.BeginHorizontal();

                methodFoldouts[i] = EditorGUILayout.Foldout(methodFoldouts[i], string.IsNullOrEmpty(methodName.stringValue) ? $"Unnamed Method {i}" : methodName.stringValue, true);
                if (GUILayout.Button("Delete", GUILayout.Width(70)))
                {
                    actionMethods.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    SyncFoldouts();
                    return;
                }

                EditorGUILayout.EndHorizontal();

                if (methodFoldouts[i])
                {
                    methodName.stringValue = EditorGUILayout.TextField("Method Name", methodName.stringValue);
                    EditorGUILayout.Space(5);

                    for (int j = 0; j < instructions.arraySize; j++)
                    {
                        SerializedProperty instruction = instructions.GetArrayElementAtIndex(j);
                        SerializedProperty stepName = instruction.FindPropertyRelative("stepName");
                        SerializedProperty stepType = instruction.FindPropertyRelative("stepType");
                        SerializedProperty gameObjectsToAppear = instruction.FindPropertyRelative("gameObjectsToAppear");
                        SerializedProperty gameObjectsToDisappear = instruction.FindPropertyRelative("gameObjectsToDisappear");
                        SerializedProperty playerPrefChanges = instruction.FindPropertyRelative("playerPrefChanges");
                        SerializedProperty itemsToChange = instruction.FindPropertyRelative("itemsToChange");
                        SerializedProperty heldItem = instruction.FindPropertyRelative("heldItem");
                        SerializedProperty removingItem = instruction.FindPropertyRelative("removingItem");
                        SerializedProperty toolTipMessage = instruction.FindPropertyRelative("toolTipData.message");
                        SerializedProperty toolTipIcon = instruction.FindPropertyRelative("toolTipData.icon");
                        SerializedProperty customEvent = instruction.FindPropertyRelative("customEvent");

                        Color stepColor = GetStepColor((AfterActionEvents.StepType)stepType.enumValueIndex);

                        GUI.backgroundColor = stepColor;
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        GUI.backgroundColor = Color.white;

                        stepFoldouts[i][j] = EditorGUILayout.Foldout(stepFoldouts[i][j], string.IsNullOrEmpty(stepName.stringValue) ? $"Unnamed Step {j}" : stepName.stringValue, true);

                        if (stepFoldouts[i][j])
                        {
                            EditorGUILayout.Space(5);
                            stepName.stringValue = EditorGUILayout.TextField("Step Name", stepName.stringValue);

                            EditorGUILayout.BeginHorizontal();
                            stepType.enumValueIndex = (int)(AfterActionEvents.StepType)EditorGUILayout.EnumPopup("Step Type", (AfterActionEvents.StepType)stepType.enumValueIndex);

                            if (GUILayout.Button("Remove Step", GUILayout.Width(100)))
                            {
                                instructions.DeleteArrayElementAtIndex(j);
                                serializedObject.ApplyModifiedProperties();
                                SyncFoldouts();
                                return;
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.Space(5);

                            AfterActionEvents.StepType currentType = (AfterActionEvents.StepType)stepType.enumValueIndex;

                            switch (currentType)
                            {
                                case AfterActionEvents.StepType.ChangeVisibility:
                                    EditorGUILayout.PropertyField(gameObjectsToAppear, new GUIContent("Objects To Appear"), true);
                                    EditorGUILayout.PropertyField(gameObjectsToDisappear, new GUIContent("Objects To Disappear"), true);
                                    break;

                                case AfterActionEvents.StepType.ChangePlayerPrefs:
                                    DrawPlayerPrefChanges(playerPrefChanges);
                                    break;

                                case AfterActionEvents.StepType.ChangeItems:
                                    DrawItemChanges(itemsToChange);
                                    break;

                                case AfterActionEvents.StepType.ChangeHeldItem:
                                    EditorGUILayout.PropertyField(heldItem, new GUIContent("Held Item"), true);
                                    removingItem.boolValue = EditorGUILayout.Toggle("Removing Item?", removingItem.boolValue);
                                    break;

                                case AfterActionEvents.StepType.ToolTip:
                                    // Handle Tooltip
                                    EditorGUILayout.PropertyField(toolTipMessage, new GUIContent("Tooltip Message"));
                                    EditorGUILayout.PropertyField(toolTipIcon, new GUIContent("Tooltip Icon"));
                                    break;

                                case AfterActionEvents.StepType.CustomEvent:
                                    // Handle Custom Event
                                    EditorGUILayout.PropertyField(customEvent, new GUIContent("Custom Event"));
                                    break;
                            }
                        }

                        EditorGUILayout.EndVertical();
                        GUILayout.Space(4);
                    }

                    if (GUILayout.Button("Add Instruction Step"))
                    {
                        instructions.arraySize++;
                        serializedObject.ApplyModifiedProperties();
                        SyncFoldouts();
                        return;
                    }
                }

                EditorGUILayout.EndVertical();
                GUILayout.Space(10);

                // Add a separator line between each action method
                DrawSeparatorLine();
            }

            if (GUILayout.Button("Add Action Method"))
            {
                actionMethods.arraySize++;
                serializedObject.ApplyModifiedProperties();
                SyncFoldouts();
                return;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSeparatorLine()
    {
        var rect = EditorGUILayout.GetControlRect(false, 2); // 2 is for the height of the line
        rect.height = 2;
        EditorGUI.DrawRect(rect, Color.gray);
        EditorGUILayout.Space(10); // Add some space after the line
    }

    private void DrawItemChanges(SerializedProperty itemsToChange)
    {
        EditorGUILayout.LabelField("Items To Change", EditorStyles.boldLabel);

        for (int i = 0; i < itemsToChange.arraySize; i++)
        {
            SerializedProperty itemChange = itemsToChange.GetArrayElementAtIndex(i);
            SerializedProperty itemName = itemChange.FindPropertyRelative("itemName");
            SerializedProperty itemQuantity = itemChange.FindPropertyRelative("quantity");
            SerializedProperty itemType = itemChange.FindPropertyRelative("itemType");

            EditorGUILayout.BeginHorizontal();
            itemName.stringValue = EditorGUILayout.TextField(itemName.stringValue, GUILayout.Width(150));
            itemQuantity.intValue = EditorGUILayout.IntField(itemQuantity.intValue, GUILayout.Width(70));

            // ItemType Dropdown
            itemType.enumValueIndex = EditorGUILayout.Popup(itemType.enumValueIndex, System.Enum.GetNames(typeof(InventorySystem.ItemType)));

            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                itemsToChange.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Item Change"))
        {
            itemsToChange.arraySize++;
        }
    }

    private void DrawPlayerPrefChanges(SerializedProperty playerPrefChanges)
    {
        EditorGUILayout.LabelField("Player Pref Changes", EditorStyles.boldLabel);

        for (int i = 0; i < playerPrefChanges.arraySize; i++)
        {
            SerializedProperty change = playerPrefChanges.GetArrayElementAtIndex(i);
            SerializedProperty key = change.FindPropertyRelative("key");
            SerializedProperty value = change.FindPropertyRelative("value");

            EditorGUILayout.BeginHorizontal();
            key.stringValue = EditorGUILayout.TextField(key.stringValue, GUILayout.Width(150));
            value.intValue = EditorGUILayout.IntField(value.intValue, GUILayout.Width(70));

            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                playerPrefChanges.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Player Pref Change"))
        {
            playerPrefChanges.arraySize++;
        }
    }

    private Color GetStepColor(AfterActionEvents.StepType type)
    {
        switch (type)
        {
            case AfterActionEvents.StepType.ChangeVisibility:
                return new Color(0.6f, 0.8f, 1f);
            case AfterActionEvents.StepType.ChangePlayerPrefs:
                return new Color(1f, 1f, 0.6f);
            case AfterActionEvents.StepType.ChangeItems:
                return new Color(0.6f, 1f, 0.6f);
            case AfterActionEvents.StepType.ChangeHeldItem:
                return new Color(0.6f, 1f, 1f);
            case AfterActionEvents.StepType.ToolTip:
                return new Color(0.8f, 0.8f, 0.8f);  // Grey for ToolTip
            case AfterActionEvents.StepType.CustomEvent:
                return new Color(1f, 0.8f, 0.8f);  // Light red for CustomEvent
            default:
                return Color.white;
        }
    }
}
