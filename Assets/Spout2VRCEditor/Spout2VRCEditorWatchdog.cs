#if UNITY_EDITOR && !UDONSHARP_COMPILER
using Klak.Spout;
using UnityEditor;
using UnityEngine;

// ensure class initializer is called whenever scripts recompile
namespace Spout2VRCEditor
{
    [InitializeOnLoad]
    public class Spout2VRCEditorWatchdog : Editor
    {
        private static GameObject _spout2VrcEditor;
        public static SpoutReceiver SpoutReceiver;
    
        private static bool _isCallbackActive;
    
        public static string Title => nameof(Spout2VRCEditorWatchdog);

        // register an event handler when the class is initialized
        static Spout2VRCEditorWatchdog()
        {
            WatchPlayModeState();
        }

        private static void WatchPlayModeState()
        {
            if (_isCallbackActive) return;
            Debug.Log(nameof(Spout2VRCEditorWatchdog) + " is now watching PlayMode state");
            _isCallbackActive = true;
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
        }

        private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredPlayMode)
            {
                if (_spout2VrcEditor != null) DestroyImmediate(_spout2VrcEditor);

                var targets = Spout2VRCEditorSettings.GetDefaultSettings();
            
                for (var i = 0; i < targets.pipes.Count; i++)
                    Spout2VRCEditorCore.InitializeSpout(targets.pipes[i].renderTexture, targets.pipes[i].sourceName, out _spout2VrcEditor);
            
                Debug.Log($"{Title} created {targets.pipes.Count} spout receivers");
            }
            else if (obj == PlayModeStateChange.ExitingPlayMode)
            {
                // Clear spout2
                if (_spout2VrcEditor != null) DestroyImmediate(_spout2VrcEditor);
            }
        }
    }
}
#endif