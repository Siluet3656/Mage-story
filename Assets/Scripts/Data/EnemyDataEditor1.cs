#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CustomEditor(typeof(EnemyData))]
    public class EnemyDataEditor1 : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EnemyData enemyData = target as EnemyData;
            
            if (GUILayout.Button("Update data"))
            {
                if (enemyData != null) enemyData.UpdateData();
            }
        }
    }
}
#endif