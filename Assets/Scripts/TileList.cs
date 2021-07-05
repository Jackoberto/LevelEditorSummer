using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "LevelEditor/TileList")]
public class TileList : ScriptableObject
{
    [SerializeField] private Tile[] tiles;
    public Tile[] Tiles => tiles;
    public Tile this[int index] => Tiles[index];
}