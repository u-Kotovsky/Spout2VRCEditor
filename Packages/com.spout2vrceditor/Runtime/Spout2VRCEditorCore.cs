#if UNITY_EDITOR && !UDONSHARP_COMPILER
using System.Collections.Generic;
using Klak.Spout;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Spout2VRCEditor
{
    public static class Spout2VRCEditorCore
    {
        public static SpoutReceiver AddSpoutReceiver(ref GameObject gameObject, ref RenderTexture targetRenderTexture)
        {
            var spoutReceiver = gameObject.AddComponent<SpoutReceiver>();
            spoutReceiver.enabled = false;
            spoutReceiver.targetTexture = targetRenderTexture;
            spoutReceiver.enabled = true;
            return spoutReceiver;
        }

        public static void InitializeSpout(RenderTexture targetRenderTexture, string targetSourceName, out GameObject gameObject)
        {
            Debug.Log($"Initializing Spout Receiver for \"{targetRenderTexture.name}\" pipes from \"{targetSourceName}\"");
            gameObject = new GameObject($"Spout2VRCEditor ({targetRenderTexture.name} <- {targetSourceName})");
            var spoutReceiver = AddSpoutReceiver(ref gameObject, ref targetRenderTexture);
            spoutReceiver.sourceName = targetSourceName;
        }
    
        public static IEnumerable<T> FindAssetsByType<T>() where T : Object {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        
            foreach (var t in guids) {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) {
                    yield return asset;
                }
            }
        }
    }
}
#endif