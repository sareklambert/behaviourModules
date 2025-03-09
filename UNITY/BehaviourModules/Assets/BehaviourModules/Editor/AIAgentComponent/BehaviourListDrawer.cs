using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using BehaviourModules.Core;
using static BehaviourModules.Editor.AIAgentComponent.AIAgentComponentValidator;

namespace BehaviourModules.Editor.AIAgentComponent
{
    /// <summary>
    /// Part of the AIAgentComponent editor UI. Displays a list of behaviours.
    /// </summary>
    public class BehaviourListDrawer : ReorderableListDrawer
    {
        private const string PropertyNameBehaviour = "behaviour";
        private const string PropertyNameWeight = "weight";
        
        private readonly AIAgentComponentValidator m_validator;
        
        public BehaviourListDrawer(SerializedObject obj, SerializedProperty prop, AIAgentComponentValidator validator)
            : base(obj, prop)
        {
            m_validator = validator;
        }

        protected override ReorderableList CreateList()
        {
            return new ReorderableList(serializedObject, property, true, true, true, true)
            {
                // Display header
                drawHeaderCallback = rect =>
                {
                    if (m_validator.ErrorId == ConfigError.BehaviourListEmpty) GUI.color = Color.red;
                    EditorGUI.LabelField(rect, "Behaviours & Weights");
                    GUI.color = Color.white;
                },
                
                // Display list elements
                drawElementCallback = (rect, index, _, _) =>
                {
                    SerializedProperty element = property.GetArrayElementAtIndex(index);
                    SerializedProperty behaviourProperty = element.FindPropertyRelative(PropertyNameBehaviour);
                    SerializedProperty weightProperty = element.FindPropertyRelative(PropertyNameWeight);
                    float fieldWidth = rect.width / 2 - 5;

                    if (m_validator.ErrorId == ConfigError.NoBehaviourObjectAssigned && m_validator.ErrorListIndex == index)
                        GUI.color = Color.red;
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight),
                        behaviourProperty, typeof(AIBehaviour), GUIContent.none);
                    GUI.color = Color.white;

                    EditorGUI.PropertyField(new Rect(rect.x + fieldWidth + 10, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight),
                        weightProperty, GUIContent.none);
                },
                
                // Get element height
                elementHeightCallback = _ => EditorGUIUtility.singleLineHeight,

                // Add element
                onAddCallback = list =>
                {
                    int newIndex = list.serializedProperty.arraySize;
                    list.serializedProperty.arraySize++;
                    serializedObject.ApplyModifiedProperties();

                    SerializedProperty newElement = list.serializedProperty.GetArrayElementAtIndex(newIndex);
                    newElement.FindPropertyRelative(PropertyNameBehaviour).objectReferenceValue = null;
                    newElement.FindPropertyRelative(PropertyNameWeight).intValue = 1;
                }
            };
        }
    }
}
