using UnityEditor;

namespace BehaviourModules.Editor.AIAgentComponent
{
    /// <summary>
    /// Helper class for the AIAgentComponent editor UI.
    /// </summary>
    public class AIAgentEditorUI
    {
        private const string PropertyNameStates = "states";
        private const string PropertyNameStateNamesList = "stateNamesList";
        
        private readonly SerializedObject m_aiAgentObject;
        private readonly SerializedProperty m_statesProperty;
        private readonly SerializedProperty m_stateNamesProperty;
        
        private ReorderableListDrawer m_statesList;

        public AIAgentEditorUI(SerializedObject aiAgentObject)
        {
            m_aiAgentObject = aiAgentObject;
            m_statesProperty = aiAgentObject.FindProperty(PropertyNameStates);
            m_stateNamesProperty = aiAgentObject.FindProperty(PropertyNameStateNamesList);
        }

        public void Draw()
        {
            // Display field for assigning the state names list
            EditorGUILayout.PropertyField(m_stateNamesProperty);
            EditorGUILayout.Separator();
            
            // Create and display the state list
            if (!m_stateNamesProperty.objectReferenceValue) return;
            m_statesList ??= new StateListDrawer(m_aiAgentObject, m_statesProperty, m_stateNamesProperty);
            m_statesList.DrawLayout();
        }
    }
}
