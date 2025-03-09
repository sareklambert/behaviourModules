using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace BehaviourModules.Editor.AIAgentComponent
{
    /// <summary>
    /// Helper class for displaying reorderable lists in the editor UI.
    /// </summary>
    public abstract class ReorderableListDrawer
    {
        protected readonly SerializedObject serializedObject;
        protected readonly SerializedProperty property;
        private ReorderableList m_list;

        protected ReorderableListDrawer(SerializedObject obj, SerializedProperty prop)
        {
            serializedObject = obj;
            property = prop;
            InitializeList();
        }

        private void InitializeList()
        {
            m_list = CreateList();
        }

        protected abstract ReorderableList CreateList();

        public void Draw(Rect rect) => m_list.DoList(rect);
        public void DrawLayout() => m_list.DoLayoutList();
    }
}
