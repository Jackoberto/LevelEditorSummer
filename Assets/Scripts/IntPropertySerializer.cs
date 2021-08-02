using System;

public class IntPropertySerializer : EditorPropertySerializer
{
    public override Type SerializedType => typeof(int);

    public override bool IsValid(string value)
    {
        return int.TryParse(value, out var i);
    }

    public override string Serialize(object value)
    {
        var intValue = (int)value;
        return intValue.ToString();
    }

    public override object Deserialize(string value)
    {
        return int.Parse(value);
    }
}