using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditorPrefabVisual : MonoBehaviour
{
    [NonSerialized] public EditorPrefab EditorPrefab;
    public List<SerializedProperty> SerializedProperties;

    public void Setup()
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
                    field.GetCustomAttributes(typeof(EditorPropertyAttribute), true).Length == 1)
                .Where(field => PropertySerializers.Dictionary.ContainsKey(field.FieldType));
            foreach (var field in fieldInfos)
            {
                SerializedProperties.Add(new SerializedProperty
                {
                    value = PropertySerializers.Dictionary[field.FieldType].Serialize(field.GetValue(monoBehaviour)),
                    componentAssembly = monoBehaviour.GetType().Assembly.ToString(),
                    componentName = monoBehaviour.GetType().ToString(),
                    typeName = field.FieldType.ToString(),
                    typeAssembly = field.FieldType.Assembly.ToString(),
                    name = field.Name
                });
            }
        }
    }
}