using System;

/// <summary>
/// Base class for defining serialization of EditorProperty fields.<para/>
/// Requires inheriting classes to have a parameterless constructor
/// </summary>
public interface IEditorPropertySerializer
{
    public Type SerializedType { get; }
    
    public bool IsValid(string value);

    public string Serialize(object value);

    public object Deserialize(string value);
}