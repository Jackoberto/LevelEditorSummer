using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Saving : MonoBehaviour
{
    public TileList tileList;
    public EditorPrefabList prefabList;
    private ISaveSystem saveSystem;

    private void Awake()
    {
        saveSystem = new SaveSystem();
    }
    
    public void Save(string mapName)
    {
        var maps = GetComponentsInChildren<Tilemap>();
        var saveFile = new SaveFile(maps, FindObjectsOfType<EditorPrefabVisual>().Where(prefab => prefab.EditorPrefab != null));
        var json = JsonUtility.ToJson(saveFile);
        saveSystem.Save(mapName, json);
    }
    
    public void Load(string mapName)
    {
        var json = saveSystem.Load(mapName);
        var saveFile = JsonUtility.FromJson<SaveFile>(json);
        PopulateLevel(saveFile);
    }
    
    public void LoadInEditor(string mapName)
    {
        var json = saveSystem.Load(mapName);
        var saveFile = JsonUtility.FromJson<SaveFile>(json);
        PopulateLevelInEditor(saveFile);
    }

    public string[] GetAllMapNames()
    {
        return saveSystem.GetAllMapNames();
    }
    
    private void PopulateLevelInEditor(SaveFile saveFile)
    {
        var maps = GetComponentsInChildren<Tilemap>();
        var currentPrefabs = FindObjectsOfType<EditorPrefabVisual>();
        foreach (var editorPrefab in currentPrefabs)
        {
            if (editorPrefab.EditorPrefab != null)
                Destroy(editorPrefab.gameObject);
        }
        foreach (var serializedEditorPrefab in saveFile.editorPrefabs)
        {
            if (prefabList.PrefabDictionary.TryGetValue(serializedEditorPrefab.id, out var editorPrefab))
            {
                var instance = Instantiate(editorPrefab.editorPrefabVisual, serializedEditorPrefab.position, editorPrefab.prefab.transform.rotation);
                instance.EditorPrefab = editorPrefab;
            }
        }

        for (var i = 0; i < saveFile.tileMaps.Count; i++)
        {
            PopulateTileMap(maps[i], saveFile.tileMaps[i]);
        }
    }

    private void PopulateLevel(SaveFile saveFile)
    {
        var maps = GetComponentsInChildren<Tilemap>();
        foreach (var serializedEditorPrefab in saveFile.editorPrefabs)
        {
            if (prefabList.PrefabDictionary.TryGetValue(serializedEditorPrefab.id, out var editorPrefab))
            {
                Instantiate(editorPrefab.prefab, serializedEditorPrefab.position, editorPrefab.prefab.transform.rotation);
            }
        }

        for (var i = 0; i < saveFile.tileMaps.Count; i++)
        {
            PopulateTileMap(maps[i], saveFile.tileMaps[i]);
        }
    }

    private void PopulateTileMap(Tilemap tilemap, SerializedTileMap serializedTileMap)
    {
        tilemap.ClearAllTiles();
        foreach (var serializedTile in serializedTileMap.serializedTiles)
        {
            if (tileList.TileDictionary.TryGetValue(serializedTile.tileId, out var tile))
            {
                tilemap.SetTile(serializedTile.position + serializedTileMap.origin, tile);
            }
        }
    }
}