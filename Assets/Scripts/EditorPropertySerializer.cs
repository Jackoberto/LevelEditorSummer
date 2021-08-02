using System;

public abstract class EditorPropertySerializer
{
    public abstract Type SerializedType { get; }
    
    public abstract bool IsValid(string value);

    public abstract string Serialize(object value);

    public abstract object Deserialize(string value);
}