using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using BehaviourModules.Core;

namespace BehaviourModules.Editor
{
    /// <summary>
    /// Editor for the state names list.
    /// </summary>
    [CustomEditor(typeof(AIStateNamesList))]
    public class AIStateNamesListEditor : UnityEditor.Editor
    {
        private const string PropertyNameStateNames = "stateNames";
        
        private SerializedProperty m_stateNamesProperty;
        private ReorderableList m_stateNamesItemsList;
        private AIStateNamesList m_aiStateNamesList;
        
        private void OnEnable()
        {
            m_aiStateNamesList = (AIStateNamesList) target;
            m_stateNamesProperty = serializedObject.FindProperty(PropertyNameStateNames);

            m_stateNamesItemsList = new ReorderableList(serializedObject, m_stateNamesProperty)
            {
                displayAdd = true,
                displayRemove = true,
                draggable = true,

                // Display header
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, m_stateNamesProperty.displayName),

                // Display list elements
                drawElementCallback = (rect, index, _, _) =>
                {
                    SerializedProperty element = m_stateNamesProperty.GetArrayElementAtIndex(index);
                    string[] availableIDs = m_aiStateNamesList.StateNames;

                    // Display field for the state name
                    Color color = GUI.color;
                    if (string.IsNullOrWhiteSpace(element.stringValue) || availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1)
                    {
                        GUI.color = Color.red;
                    }
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element);
                    GUI.color = color;

                    // Display HelpBox
                    if (string.IsNullOrWhiteSpace(element.stringValue))
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Name may not be empty!", MessageType.Error );
                    }
                    else if (availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1)
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Duplicate! Name has to be unique!", MessageType.Error );
                    }
                },

                // Get element height
                elementHeightCallback = index =>
                {
                    SerializedProperty element = m_stateNamesProperty.GetArrayElementAtIndex(index);
                    string[] availableIDs = m_aiStateNamesList.StateNames;

                    float height = EditorGUI.GetPropertyHeight(element);

                    if (string.IsNullOrWhiteSpace(element.stringValue) || availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1)
                    {
                        height += EditorGUIUtility.singleLineHeight;
                    }

                    return height;
                },

                // Add element
                onAddCallback = list =>
                {
                    list.serializedProperty.arraySize++;

                    SerializedProperty newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                    newElement.stringValue = "";
                }
            };
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            m_stateNamesItemsList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
