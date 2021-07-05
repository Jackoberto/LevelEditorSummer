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
            Debug.Log(texture2D);
            var sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                  new Vector2(.5f, .5f));
            UnityEditor.AssetDatabase.CreateAsset(sprite, $"Assets/Resources/PreviewImages/{prefab.name}_Preview.asset");
            previewImage = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Resources/PreviewImages/{prefab.name}_Preview.asset");
      }
      #endif
}