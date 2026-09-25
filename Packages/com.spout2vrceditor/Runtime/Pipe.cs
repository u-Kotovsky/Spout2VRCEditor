using System;
using JetBrains.Annotations;
using UnityEngine;
#if UNITY_EDITOR && !UDONSHARP_COMPILER
using Klak.Spout;
using UnityEditor;
#endif

namespace Spout2VRCEditor
{
    [Serializable]
    public class Pipe
    {
        [CanBeNull]
        public RenderTexture renderTexture;
        public string sourceName;
        
#if UNITY_EDITOR && !UDONSHARP_COMPILER
        [NonSerialized] public SpoutReceiver spoutReceiver;
        public void OnInspectorGUI()
        {
            //var serializedObject = new SerializedObject(this);
             
            EditorGUI.BeginChangeCheck();
            
            //serializedObject.FindProperty(nameof(renderTexture)).objectReferenceValue = EditorGUILayout.ObjectField(renderTexture, typeof(RenderTexture), false) as RenderTexture;
            //serializedObject.FindProperty(nameof(sourceName)).stringValue =  EditorGUILayout.TextField(sourceName);
            
            renderTexture = EditorGUILayout.ObjectField(renderTexture, typeof(RenderTexture), false) as RenderTexture;
            sourceName =  EditorGUILayout.TextField(sourceName);
                
            var rect = EditorGUILayout.GetControlRect(false, GUILayout.Width(60));
            if (EditorGUI.DropdownButton(rect, new GUIContent("Select"), FocusType.Keyboard))
                ShowSourceNameDropdown(rect);
            //if (EditorGUI.EndChangeCheck())
            //{
                //serializedObject.ApplyModifiedProperties();
            //}

        }
        
        // Create and show the source name dropdown.
        void ShowSourceNameDropdown(Rect rect)
        {
            var menu = new GenericMenu();
            var sources = SpoutManager.GetSourceNames();

            if (sources.Length > 0)
            {
                foreach (var name in sources)
                    menu.AddItem(new GUIContent(name), false, OnSelectSource, name);
            }
            else
            {
                menu.AddItem(new GUIContent("No source available"), false, null);
            }

            menu.DropDown(rect);
        }

        // Source name selection callback
        void OnSelectSource(object nameObject)
        {
            var name = (string)nameObject;
            
            sourceName = name;
        
            //serializedObject.Update();
            //_sourceName.stringValue = name;
            //serializedObject.ApplyModifiedProperties();
            RequestRestart();
        }

        // Receiver restart request
        void RequestRestart()
        {
            // Dirty trick: We only can restart receivers by modifying the
            // sourceName property, so we modify it by an invalid name, then
            // revert it.
            if (spoutReceiver == null) return;
            spoutReceiver.sourceName = "";
            spoutReceiver.sourceName = sourceName;
        }
#endif
    }
}
