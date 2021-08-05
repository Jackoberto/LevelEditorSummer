using System;

public class IntPropertySerializer : IEditorPropertySerializer
{
    public Type SerializedType => typeof(int);

    public bool IsValid(string value)
    {
        return int.TryParse(value, out var i);
    }

    public string Serialize(object value)
    {
        var intValue = (int)value;
        return intValue.ToString();
    }

    public object Deserialize(string value)
    {
        return int.Parse(value);
    }
}