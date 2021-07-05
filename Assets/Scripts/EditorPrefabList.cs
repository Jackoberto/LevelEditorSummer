using UnityEngine;

[CreateAssetMenu(menuName = "LevelEditor/EditorPrefabList")]
public class EditorPrefabList : ScriptableObject
{
    [SerializeField] private EditorPrefab[] prefabs;
    public EditorPrefab[] Prefabs => prefabs;
    public EditorPrefab this[int index] => Prefabs[index];
}