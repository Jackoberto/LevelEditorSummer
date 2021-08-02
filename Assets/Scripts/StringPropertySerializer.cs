using System;

public class StringPropertySerializer : EditorPropertySerializer
{
    public override Type SerializedType => typeof(string);

    public override bool IsValid(string value)
    {
        return true;
    }

    public override string Serialize(object value)
    {
        return value.ToString();
    }

    public override object Deserialize(string value)
    {
        return value;
    }
}