using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditorPrefabVisual : MonoBehaviour
{
    [NonSerialized] public EditorPrefab EditorPrefab;
    public List<SerializedProperty> SerializedProperties;

    private void Start()
    {
        if (EditorPrefab != null && EditorPrefab.prefab != null)
            GenerateSerializedProperties();
    }

    private void GenerateSerializedProperties()
    {
        var monoBehaviours = EditorPrefab.prefab.GetComponents<MonoBehaviour>();
        foreach (var monoBehaviour in monoBehaviours)
        {
            var fieldInfos = monoBehaviour
                .GetType()
                .GetFields()
                .Where(field =>
                    field.GetCustomAttributes(typeof(EditorPropertyAttribute), true).Length == 1);
            foreach (var field in fieldInfos)
            {
                SerializedProperties.Add(new SerializedProperty
                {
                    value = PropertySerializers.Dictionary[field.FieldType].Serialize(field.GetValue(monoBehaviour)),
                    component = monoBehaviour.GetType().ToString(),
                    type = field.FieldType.ToString()
                });
            }
        }
    }
}