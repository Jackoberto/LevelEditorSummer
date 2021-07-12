using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class SaveFile
{
    public SaveFile(IEnumerable<Tilemap> tileMaps, IEnumerable<EditorPrefab> editorPrefabs)
    {
        this.tileMaps = tileMaps.Select(tileMap => new SerializedTileMap(tileMap)).ToList();
        this.editorPrefabs = editorPrefabs.Select(editorPrefab => editorPrefab.id).ToList();
    }
    public List<SerializedTileMap> tileMaps;
    public List<string> editorPrefabs;
}

[Serializable]
public class SerializedTileMap
{
    public SerializedTileMap(Tilemap tilemap)
    {
        origin = new SerializedVector2(tilemap.origin);
        for (var i = 0; i < tilemap.size.x; i++)
        {
            for (var j = 0; j < tilemap.size.y; j++)
            {
                var tile = tilemap.GetTile(new Vector3Int(origin.x + i, origin.y + j, 0));
                if (tile == null) continue;
                serializedTiles.Add(new SerializedTile(new SerializedVector2(i, j), tile));
            }
        }
    }

    public SerializedVector2 origin;
    public List<SerializedTile> serializedTiles = new List<SerializedTile>();
}

[Serializable]
public struct SerializedTile
{
    public SerializedVector2 position;
    public string tileId;
    public SerializedTile(SerializedVector2 serializedVector2, TileBase tile)
    {
        position = serializedVector2;
        tileId = tile.name;
    }
}

[Serializable]
public struct SerializedVector2
{
    public int x;
    public int y;
    public SerializedVector2(Vector2Int vector2Int)
    {
        x = vector2Int.x;
        y = vector2Int.y;
    }
    public SerializedVector2(Vector3Int vector3Int)
    {
        x = vector3Int.x;
        y = vector3Int.y;
    }
    public SerializedVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}