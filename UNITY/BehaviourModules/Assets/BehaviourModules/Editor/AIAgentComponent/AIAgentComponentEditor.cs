using UnityEditor;

namespace BehaviourModules.Editor.AIAgentComponent
{
    /// <summary>
    /// Displays reorderable lists for states with sub lists for behaviours and conditions.
    /// </summary>
    [CustomEditor(typeof(Core.AIAgentComponent))]
    public class AIAgentComponentEditor : UnityEditor.Editor
    {
        private AIAgentEditorUI m_editorUI;

        private void OnEnable()
        {
            m_editorUI = new AIAgentEditorUI(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            m_editorUI.Draw();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
