using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelEditor/EditorPrefabList")]
public class EditorPrefabList : ScriptableObject
{
    [SerializeField] private EditorPrefab[] prefabs;
    public EditorPrefab[] Prefabs => prefabs;
    public Dictionary<string, EditorPrefab> PrefabDictionary
    {
        get { return _editorPrefabs ??= Prefabs.ToDictionary(p => p.id); }
    }
    public EditorPrefab this[int index] => Prefabs[index];
    private Dictionary<string, EditorPrefab> _editorPrefabs;
}