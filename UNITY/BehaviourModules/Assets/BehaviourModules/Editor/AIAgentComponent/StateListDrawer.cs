using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using BehaviourModules.Core;
using static BehaviourModules.Editor.AIAgentComponent.AIAgentComponentValidator;

namespace BehaviourModules.Editor.AIAgentComponent
{
    /// <summary>
    /// Part of the AIAgentComponent editor UI. Displays a list of states.
    /// </summary>
    public class StateListDrawer : ReorderableListDrawer
    {
        private const string PropertyNameStateId = "stateId";
        private const string PropertyNameBehaviours = "behaviours";
        private const string PropertyNameConditions = "conditions";
        private const string PropertyNameTriggers = "triggers";
        private const string PropertyNameParent = "parent";
        
        private readonly Dictionary<string, BehaviourListDrawer> m_behaviourLists = new Dictionary<string, BehaviourListDrawer>();
        private readonly Dictionary<string, ConditionListDrawer> m_conditionLists = new Dictionary<string, ConditionListDrawer>();
        private readonly AIStateNamesList m_stateNamesList;
        private readonly AIAgentComponentValidator m_validator;
        
        public StateListDrawer(SerializedObject obj, SerializedProperty prop, SerializedProperty stateNamesList)
            : base(obj, prop)
        {
            m_stateNamesList = stateNamesList.objectReferenceValue as AIStateNamesList;
            m_validator = new AIAgentComponentValidator();
        }

        protected override ReorderableList CreateList()
        {
            return new ReorderableList(serializedObject, property, true, true, true, true)
            {
                // Display header
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, property.displayName),
                
                // Display list elements
                drawElementCallback = (rect, index, _, _) =>
                {
                    // Get the current element's SerializedProperties
                    SerializedProperty state = property.GetArrayElementAtIndex(index);
                    SerializedProperty stateId = state.FindPropertyRelative(PropertyNameStateId);
                    SerializedProperty behaviourList = state.FindPropertyRelative(PropertyNameBehaviours);
                    SerializedProperty conditionList = state.FindPropertyRelative(PropertyNameConditions);

                    // Get unique keys using property paths
                    string behaviourKey = behaviourList.propertyPath;
                    string conditionKey = conditionList.propertyPath;

                    // Check for errors
                    m_validator.CheckForErrors(property, index);
                    
                    // Display foldout with state name as label
                    GUIStyle style = new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold };
                    if (m_validator.ErrorId != ConfigError.None)
                        GUI.color = Color.red;
                    state.isExpanded = EditorGUI.Foldout(
                        new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        state.isExpanded,
                        stateId.intValue == -1 ? "Undefined" : m_stateNamesList!.StateNames[stateId.intValue],
                        style
                    );
                    GUI.color = Color.white;
                    
                    // Display expanded view
                    if (!state.isExpanded) return;

                    // State name dropdown
                    rect.y += EditorGUIUtility.singleLineHeight;
                    
                    if (m_validator.ErrorId is ConfigError.NoStateName or ConfigError.StateNameAssignedMultipleTimes)
                        GUI.color = Color.red;
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Name");
                    GUI.color = Color.white;

                    int newStateId = EditorGUI.Popup(
                        new Rect(rect.x + 80, rect.y, rect.width - 80, EditorGUIUtility.singleLineHeight),
                        stateId.intValue,
                        m_stateNamesList!.StateNames
                    );
                    if (stateId.intValue != newStateId) stateId.intValue = newStateId;
                    
                    // Display behaviours list
                    rect.y += EditorGUIUtility.singleLineHeight * 2;

                    if (!m_behaviourLists.ContainsKey(behaviourKey))
                    {
                        m_behaviourLists[behaviourKey] = new BehaviourListDrawer(serializedObject, behaviourList, m_validator);
                    }
                    m_behaviourLists[behaviourKey].Draw(rect);

                    // Display conditions list
                    rect.y += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) *
                              (Mathf.Max(1, behaviourList.arraySize) + 3);

                    if (!m_conditionLists.ContainsKey(conditionKey))
                    {
                        m_conditionLists[conditionKey] = new ConditionListDrawer(serializedObject, conditionList, m_stateNamesList, m_validator);
                    }
                    m_conditionLists[conditionKey].Draw(rect);

                    // Display help box
                    if (m_validator.ErrorId == ConfigError.None) return;

                    if (conditionList.arraySize > 0) rect.y -= EditorGUIUtility.singleLineHeight;
                    for (int i = 0; i < conditionList.arraySize; i++)
                    {
                        SerializedProperty conditionProperty = conditionList.GetArrayElementAtIndex(i);
                        SerializedProperty triggerListProperty = conditionProperty.FindPropertyRelative(PropertyNameTriggers);

                        rect.y += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
                        rect.y += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) *
                                  (Mathf.Max(1, triggerListProperty.arraySize) + 2);
                    }

                    rect.y += EditorGUIUtility.singleLineHeight * 4;

                    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2),
                        m_validator.ErrorMessage, MessageType.Error);
                },
                
                // Get element height
                elementHeightCallback = index =>
                {
                    SerializedProperty state = property.GetArrayElementAtIndex(index);
                    SerializedProperty behaviourList = state.FindPropertyRelative(PropertyNameBehaviours);
                    SerializedProperty conditionList = state.FindPropertyRelative(PropertyNameConditions);

                    // Base height
                    float height = EditorGUIUtility.singleLineHeight;

                    // Add dropdown
                    if (!state.isExpanded) return height;

                    // Add spacing
                    height += EditorGUIUtility.singleLineHeight * 6;

                    // Add behaviours list
                    height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) *
                              (Mathf.Max(1, behaviourList.arraySize) + 3);

                    // Add conditions list
                    if (conditionList.arraySize > 0) height -= EditorGUIUtility.singleLineHeight;
                    for (int i = 0; i < conditionList.arraySize; i++)
                    {
                        SerializedProperty conditionProperty = conditionList.GetArrayElementAtIndex(i);
                        SerializedProperty triggersListProperty = conditionProperty.FindPropertyRelative(PropertyNameTriggers);

                        height += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
                        height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) *
                                  (Mathf.Max(1, triggersListProperty.arraySize) + 2);
                    }

                    // Add help box
                    m_validator.CheckForErrors(property, index);
                    if (m_validator.ErrorId != ConfigError.None)
                    {
                        height += EditorGUIUtility.singleLineHeight * 3;
                    }

                    return height;
                },

                // Add element
                onAddCallback = list =>
                {
                    int newIndex = list.serializedProperty.arraySize;
                    list.serializedProperty.arraySize++;
                    serializedObject.ApplyModifiedProperties();

                    SerializedProperty newElement = list.serializedProperty.GetArrayElementAtIndex(newIndex);
                    newElement.FindPropertyRelative(PropertyNameStateId).intValue = -1;
                    newElement.FindPropertyRelative(PropertyNameBehaviours).ClearArray();
                    newElement.FindPropertyRelative(PropertyNameConditions).ClearArray();
                    newElement.FindPropertyRelative(PropertyNameParent).objectReferenceValue = serializedObject.targetObject;
                }
            };
        }
    }
}
