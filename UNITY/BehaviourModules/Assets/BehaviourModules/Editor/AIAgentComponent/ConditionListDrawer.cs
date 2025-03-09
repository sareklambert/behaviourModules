using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using BehaviourModules.Core;
using static BehaviourModules.Editor.AIAgentComponent.AIAgentComponentValidator;

namespace BehaviourModules.Editor.AIAgentComponent
{
    /// <summary>
    /// Part of the AIAgentComponent editor UI. Displays a list of conditions.
    /// </summary>
    public class ConditionListDrawer : ReorderableListDrawer
    {
        private const string PropertyNameNextState = "nextState";
        private const string PropertyNameTriggers = "triggers";
        
        private readonly Dictionary<string, TriggerListDrawer> m_triggerLists = new Dictionary<string, TriggerListDrawer>();
        private readonly AIStateNamesList m_stateNamesList;
        private readonly AIAgentComponentValidator m_validator;
        
        public ConditionListDrawer(SerializedObject obj, SerializedProperty prop, AIStateNamesList stateNamesList, AIAgentComponentValidator validator)
            : base(obj, prop)
        {
            m_stateNamesList = stateNamesList;
            m_validator = validator;
        }

        protected override ReorderableList CreateList()
        {
            return new ReorderableList(serializedObject, property, true, true, true, true)
            {
                // Display header
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "Conditions");
                },
                
                // Display list elements
                drawElementCallback = (rect, index, _, _) =>
                {
                    SerializedProperty conditionElement = property.GetArrayElementAtIndex(index);
                    SerializedProperty nextStateProperty = conditionElement.FindPropertyRelative(PropertyNameNextState);
                    SerializedProperty triggerListProperty = conditionElement.FindPropertyRelative(PropertyNameTriggers);
                    string triggersKey = triggerListProperty.propertyPath;

                    // Next state dropdown
                    if (m_validator.ErrorId is ConfigError.NoNextStateName or ConfigError.NextStateIsStateName && m_validator.ErrorListIndex == index)
                        GUI.color = Color.red;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 80, EditorGUIUtility.singleLineHeight), "Next state");
                    int newNextStateId = EditorGUI.Popup(
                        new Rect(rect.x + 80, rect.y, rect.width - 80, EditorGUIUtility.singleLineHeight),
                        nextStateProperty.intValue,
                        m_stateNamesList.StateNames
                    );
                    GUI.color = Color.white;

                    if (nextStateProperty.intValue != newNextStateId)
                    {
                        nextStateProperty.intValue = newNextStateId;
                    }

                    // Adjust rect for Triggers list
                    rect.y += EditorGUIUtility.singleLineHeight * 2;

                    // Initialize Triggers list if needed
                    if (!m_triggerLists.ContainsKey(triggersKey))
                    {
                        m_triggerLists[triggersKey] = new TriggerListDrawer(serializedObject, triggerListProperty, m_validator, index);
                    }
                    m_triggerLists[triggersKey].Draw(rect);
                },
                
                // Get element height
                elementHeightCallback = index =>
                {
                    SerializedProperty conditionProperty = property.GetArrayElementAtIndex(index);
                    SerializedProperty triggersListProperty = conditionProperty.FindPropertyRelative(PropertyNameTriggers);

                    // Base height
                    float height = EditorGUIUtility.singleLineHeight * 3; 
                    
                    // Add triggers list height
                    height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * (Mathf.Max(1, triggersListProperty.arraySize) + 2); 
                    
                    return height;
                },
                
                // Add element
                onAddCallback = list =>
                {
                    int newIndex = list.serializedProperty.arraySize;
                    list.serializedProperty.arraySize++;
                    serializedObject.ApplyModifiedProperties();

                    SerializedProperty newElement = list.serializedProperty.GetArrayElementAtIndex(newIndex);
                    newElement.FindPropertyRelative(PropertyNameNextState).intValue = -1;
                    newElement.FindPropertyRelative(PropertyNameTriggers).ClearArray();
                }
            };
        }
    }
}
