using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using BehaviourModules.Core;
using static BehaviourModules.Editor.AIAgentComponent.AIAgentComponentValidator;

namespace BehaviourModules.Editor.AIAgentComponent
{
    /// <summary>
    /// Part of the AIAgentComponent editor UI. Displays a list of triggers.
    /// </summary>
    public class TriggerListDrawer : ReorderableListDrawer
    {
        private readonly AIAgentComponentValidator m_validator;
        private readonly int m_conditionIndex;
        
        public TriggerListDrawer(SerializedObject obj, SerializedProperty prop, AIAgentComponentValidator validator, int conditionIndex) :
            base(obj, prop)
        {
            m_validator = validator;
            m_conditionIndex = conditionIndex;
        }

        protected override ReorderableList CreateList()
        {
            return new ReorderableList(serializedObject, property, true, true, true, true)
            {
                // Display header
                drawHeaderCallback = rect =>
                {
                    if (m_validator.ErrorId == ConfigError.TriggerListEmpty && m_validator.ErrorListIndex == m_conditionIndex)
                        GUI.color = Color.red;
                    EditorGUI.LabelField(rect, "Triggers");
                    GUI.color = Color.white;
                },
                
                // Display list elements
                drawElementCallback = (rect, index, _, _) =>
                {
                    var triggerElement = property.GetArrayElementAtIndex(index);
                    if (m_validator.ErrorId == ConfigError.NoTriggerObjectAssigned && m_validator.ErrorListIndex == index)
                        GUI.color = Color.red;
                    EditorGUI.ObjectField(rect, triggerElement, typeof(AITrigger), GUIContent.none);
                    GUI.color = Color.white;
                },
                
                // Get element height
                elementHeightCallback = _ => EditorGUIUtility.singleLineHeight,
                
                // Add element
                onAddCallback = list =>
                {
                    var newIndex = list.serializedProperty.arraySize;
                    list.serializedProperty.arraySize++;
                    serializedObject.ApplyModifiedProperties();

                    var newElement = list.serializedProperty.GetArrayElementAtIndex(newIndex);
                    newElement.objectReferenceValue = null;
                }
            };
        }
    }
}
