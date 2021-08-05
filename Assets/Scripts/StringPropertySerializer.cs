using System;

public class StringPropertySerializer : IEditorPropertySerializer
{
    public Type SerializedType => typeof(string);

    public bool IsValid(string value)
    {
        return true;
    }

    public string Serialize(object value)
    {
        return value.ToString();
    }

    public object Deserialize(string value)
    {
        return value;
    }
}