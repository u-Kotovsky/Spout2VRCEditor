#if UNITY_EDITOR && !UDONSHARP_COMPILER
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomEditorGUI
{
    public Object targetObject;
    public Editor targetEditor;
    public bool allowFolding;

    public CustomEditorGUI(Object obj, bool allowFolding = true)
    {
        targetObject = obj;
        
        if (targetObject != null)
        {
            targetEditor = Editor.CreateEditor(targetObject);
        }
    }

    public bool IsFolded { get; set; }

    public void OnInspectorGUI()
    {
        if (targetObject == null)
        {
            EditorGUILayout.LabelField("No target selected");
            return;
        }

        // No editor created, though it should never happen
        //if (targetEditor == null) return;

        if (allowFolding)
        {
            IsFolded = EditorGUILayout.BeginFoldoutHeaderGroup(IsFolded, $"{targetObject.name}");
            if (!IsFolded)
            {
                targetEditor.OnInspectorGUI();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        else
        {
            targetEditor.OnInspectorGUI();
        }
    }
}
#endif