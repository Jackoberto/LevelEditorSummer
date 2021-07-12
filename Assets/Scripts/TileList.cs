using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "LevelEditor/TileList")]
public class TileList : ScriptableObject
{
    [SerializeField] private Tile[] tiles;
    public Tile[] Tiles => tiles;
    public Dictionary<string, Tile> TileDictionary
    {
        get { return _tileDictionary ??= Tiles.ToDictionary(p => p.name); }
    }

    private Dictionary<string, Tile> _tileDictionary;
    public Tile this[int index] => Tiles[index];
}