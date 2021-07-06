using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelEditor/EditorPrefab")]
public class EditorPrefab : ScriptableObject
{
      public GameObject prefab;
      [HideInInspector] public Sprite previewImage;

      #if UNITY_EDITOR
      private void OnValidate()
      {
            if (prefab == null)
                  return;
            if (previewImage == null)
            {
                  CreateSprite();
            }
      }
      
      [ContextMenu("Recreate Preview Sprite")]
      private void CreateSprite()
      {
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources"))
                  UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources/PreviewImages"))
                  UnityEditor.AssetDatabase.CreateFolder("Assets/Resources", "PreviewImages");
            var texture2D = UnityEditor.AssetPreview.GetAssetPreview(prefab);
            var filePath = $"Resources/PreviewImages/{name}_Preview";
            File.WriteAllBytes(Path.Combine(Application.dataPath, $"{filePath}.png"),texture2D.EncodeToPNG());
            UnityEditor.AssetDatabase.Refresh();
            previewImage = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/{filePath}.png");
      }
      #endif
}