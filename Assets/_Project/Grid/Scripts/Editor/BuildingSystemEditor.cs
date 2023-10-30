using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace upx.level
{
    [CustomEditor(typeof(BuildingSystem))]
    public class BuildingSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            
            var system = (BuildingSystem)target;

            if(GUILayout.Button("Target Focus"))
            {
                system.OnTargetFocus();
            }
            if(GUILayout.Button("Start build"))
            {
                system.StartBuild();
            }
        }
    }
}
