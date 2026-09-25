#if UNITY_EDITOR && !UDONSHARP_COMPILER
using Klak.Spout;
using UnityEditor;
using UnityEngine;

namespace Spout2VRCEditor
{
    public class Spout2VRCEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Spout2VRCEditorWindow")]
        public static void OpenWindow()
        {
            var window = GetWindow<Spout2VRCEditorWindow>();
            window.Show();
        }

        private Spout2VRCEditorSettings _settings;
    
        private SpoutReceiver[] _spoutReceivers;
        private CustomEditorGUI[] _customGUIs;

        private CustomEditorGUI _settingsGUI;
    
        private string Title => nameof(Spout2VRCEditorWindow);
    
        bool SetDefaultSettings()
        {
            _settings = Spout2VRCEditorSettings.GetDefaultSettings();
            bool found = _settings == null;
            if (found)
            {
                Debug.LogError($"{Title} was not found");
            }
            else
            {
                Debug.Log($"{Title} was found");
            }

            return found;
        }
    
        Vector2 _spoutReceiversScrollPos;
    
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Settings");
            _settings = EditorGUILayout.ObjectField(_settings, typeof(Spout2VRCEditorSettings), false) as Spout2VRCEditorSettings;
            EditorGUILayout.EndHorizontal();

            if (_settings == null)
            {
                GUILayout.Label("Settings are not selected");
                if (GUILayout.Button("Find")) SetDefaultSettings();
                return;
            }
        
            // draw settings UI
            _settingsGUI = new CustomEditorGUI(_settings, false);
            _settingsGUI.OnInspectorGUI();
        
            // draw spot receiver controls
            EditorGUI.indentLevel++;
        
            GUILayout.Label("Spout Receiver controls",  EditorStyles.boldLabel);
        
            if (EditorApplication.isPlaying)
            {
                EnsureSpoutReceiversCreated();
        
                if (_spoutReceivers == null || _spoutReceivers.Length <= 0)
                {
                    GUILayout.Label("There are no spout receivers found");
                    return;
                }

                EnsureCustomGUIsCreated();

                EditorGUILayout.BeginScrollView(_spoutReceiversScrollPos);
                foreach (var customEditorGUI in _customGUIs)
                    customEditorGUI?.OnInspectorGUI();
                EditorGUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("Controls are not available until you enter playmode.");
            }
        }

        private void EnsureSpoutReceiversCreated()
        {
            // Get all spout receivers
            if (_spoutReceivers == null)
                _spoutReceivers = FindObjectsOfType<SpoutReceiver>();
        }
    
        private void EnsureCustomGUIsCreated()
        {
            // Initialize all receivers
            if (_customGUIs == null || _customGUIs.Length != _spoutReceivers.Length)
            {
                _customGUIs = new CustomEditorGUI[_spoutReceivers.Length];

                for (var i = 0; i < _spoutReceivers.Length; i++)
                    _customGUIs[i] = new CustomEditorGUI(_spoutReceivers[i]);
            }
        }
    
    }
}
#endif