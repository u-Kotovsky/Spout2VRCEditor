#if UNITY_EDITOR && !UDONSHARP_COMPILER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Spout2VRCEditor
{
    [CreateAssetMenu(menuName = "Spout2VRCEditorSettings")]
    public class Spout2VRCEditorSettings : ScriptableObject
    {
        public List<Pipe> pipes;

        public Spout2VRCEditorSettings()
        {
            pipes = new List<Pipe>();
        }

        public static Spout2VRCEditorSettings GetDefaultSettings()
        {
            var assets = Spout2VRCEditorCore.FindAssetsByType<Spout2VRCEditorSettings>();
            return assets.FirstOrDefault();
        }
    }

    [CustomEditor(typeof(Spout2VRCEditorSettings))]
    public class Spout2VRCEditorSettings_Editor : Editor
    {
        private Vector2 _scrollPos;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        
            var asset = target as Spout2VRCEditorSettings;
            if (asset == null)
            {
                GUILayout.Label("Asset is null");
                return;
            }
        
            GUILayout.Label("Pipes",  EditorStyles.boldLabel);
        
            EditorGUILayout.BeginScrollView(_scrollPos);
            EditorGUI.indentLevel++;
            
            for (int i = 0; i < asset.pipes.Count; i++)
            {
                //string title = asset.pipes[i].renderTexture == null ? $"Element {i}" : asset.pipes[i].renderTexture.name; 
            
                /*if (asset.pipes[i].visible)
                {*/
                    EditorGUILayout.BeginHorizontal();
                    //if (GUILayout.Button(">", GUILayout.Width(20))) asset.pipes[i].visible = false;
                    asset.pipes[i].OnInspectorGUI();
                    if (GUILayout.Button("Remove")) asset.pipes.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                /*}
                else
                {
                    if (GUILayout.Button($"{title}"))
                    {
                        asset.pipes[i].visible = true;
                    }
                }*/
            }
        
            if (GUILayout.Button("Add")) asset.pipes.Add(new Pipe());
            EditorGUILayout.EndScrollView();
        }

        private bool _foldout;
        private void DrawDefaultInspector()
        {
            _foldout = EditorGUILayout.Foldout(_foldout, "Default Inspector");
            if (_foldout)
            {
                EditorGUI.indentLevel++;
                base.OnInspectorGUI();
                EditorGUI.indentLevel--;
            }
        }
    }
}
#endif