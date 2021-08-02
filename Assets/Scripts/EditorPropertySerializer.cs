using System;

/// <summary>
/// Base class for defining serialization of EditorProperty fields.<para/>
/// Requires inheriting classes to have a parameterless constructor
/// </summary>
public abstract class EditorPropertySerializer
{
    public abstract Type SerializedType { get; }
    
    public abstract bool IsValid(string value);

    public abstract string Serialize(object value);

    public abstract object Deserialize(string value);
}