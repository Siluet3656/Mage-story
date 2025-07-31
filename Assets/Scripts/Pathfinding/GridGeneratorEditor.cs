#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Pathfinding
{
    [CustomEditor(typeof(NodeGridGenerator))]
    public class GridGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        
            // Рисуем стандартные свойства
            DrawPropertiesExcluding(serializedObject, "m_Script");
        
            NodeGridGenerator generator = (NodeGridGenerator)target;
        
            EditorGUILayout.Space();
            if (GUILayout.Button("Generate Grid Now"))
            {
                Undo.RegisterCompleteObjectUndo(generator.gameObject, "Generate Grid");
                generator.GenerateGridInEditor();
                EditorUtility.SetDirty(generator);
            }
        
            if (GUILayout.Button("Clear Grid"))
            {
                Undo.RegisterCompleteObjectUndo(generator.gameObject, "Clear Grid");
                generator.ClearExistingNodes();
                EditorUtility.SetDirty(generator);
            }
        
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif