#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CustomEditor(typeof(SpellData))]
    public class SpellDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SpellData spellData = target as SpellData;
            
            if (GUILayout.Button("Update data"))
            {
                if (spellData != null) spellData.UpdateData();
            }
        }
    }
}
#endif