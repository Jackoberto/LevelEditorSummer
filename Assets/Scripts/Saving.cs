using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Saving : MonoBehaviour
{
    public string path;
    [ContextMenu("Save")]
    public void Save()
    {
        var maps = GetComponentsInChildren<Tilemap>();
        var saveFile = new SaveFile(maps, FindObjectsOfType<EditorPrefabVisual>().Where(prefab => prefab.EditorPrefab != null));
        var json = JsonUtility.ToJson(saveFile);
        File.WriteAllText(path, json);
    }
}