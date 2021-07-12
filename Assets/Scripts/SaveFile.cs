using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class SaveFile
{
    public SaveFile(IEnumerable<Tilemap> tileMaps, IEnumerable<EditorPrefabVisual> editorPrefabs)
    {
        this.tileMaps = tileMaps.Select(tileMap => new SerializedTileMap(tileMap)).ToList();
        this.editorPrefabs = editorPrefabs.Select(editorPrefab => new SerializedEditorPrefab(editorPrefab)).ToList();
    }
    public List<SerializedTileMap> tileMaps;
    public List<SerializedEditorPrefab> editorPrefabs;
}

[Serializable]
public class SerializedEditorPrefab
{
    public SerializedVector2 position;
    public string id;
    public SerializedEditorPrefab(EditorPrefabVisual editorPrefab)
    {
        position = editorPrefab.transform.position;
        id = editorPrefab.EditorPrefab.id;
    }
}

[Serializable]
public class SerializedTileMap
{
    public SerializedVector2Int origin;
    public List<SerializedTile> serializedTiles = new List<SerializedTile>();
    public SerializedTileMap(Tilemap tilemap)
    {
        origin = new SerializedVector2Int(tilemap.origin);
        for (var i = 0; i < tilemap.size.x; i++)
        {
            for (var j = 0; j < tilemap.size.y; j++)
            {
                var tile = tilemap.GetTile(new Vector3Int(origin.x + i, origin.y + j, 0));
                if (tile == null) continue;
                serializedTiles.Add(new SerializedTile(new SerializedVector2Int(i, j), tile));
            }
        }
    }
}

[Serializable]
public struct SerializedTile
{
    public SerializedVector2Int position;
    public string tileId;
    public SerializedTile(SerializedVector2Int serializedVector2Int, TileBase tile)
    {
        position = serializedVector2Int;
        tileId = tile.name;
    }
}

[Serializable]
public struct SerializedVector2Int
{
    public int x;
    public int y;
    public SerializedVector2Int(Vector2Int vector2Int)
    {
        x = vector2Int.x;
        y = vector2Int.y;
    }
    public SerializedVector2Int(Vector3Int vector3Int)
    {
        x = vector3Int.x;
        y = vector3Int.y;
    }
    public SerializedVector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator SerializedVector2Int(Vector2Int vector2Int) => new SerializedVector2Int(vector2Int);
    public static implicit operator SerializedVector2Int(Vector3Int vector3Int) => new SerializedVector2Int(vector3Int);
}

[Serializable]
public struct SerializedVector2
{
    public float x;
    public float y;
    public SerializedVector2(Vector2 vector2)
    {
        x = vector2.x;
        y = vector2.y;
    }
    public SerializedVector2(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
    }
    public SerializedVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    
    public static implicit operator SerializedVector2(Vector2 vector2) => new SerializedVector2(vector2);
    public static implicit operator SerializedVector2(Vector3 vector3) => new SerializedVector2(vector3);
}