using System;
using System.Collections.Generic;
using System.Linq;

public static class PropertySerializers
{
    private static Dictionary<Type, EditorPropertySerializer> _dictionary;

    public static Dictionary<Type, EditorPropertySerializer> Dictionary
    {
        get
        {
            if (_dictionary != null)
                return _dictionary;
            _dictionary = new Dictionary<Type, EditorPropertySerializer>();
            var propertySerializers = System.Reflection.Assembly
                .GetAssembly(typeof(EditorPropertySerializer))
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(EditorPropertySerializer)));
            foreach (var propertySerializer in propertySerializers)
            {
                var constructor = propertySerializer.GetConstructor(Type.EmptyTypes);
                if (constructor == null)
                    continue;
                var instancedSerializer = (EditorPropertySerializer) constructor.Invoke(Array.Empty<object>());
                var type = instancedSerializer.SerializedType;
                _dictionary.Add(type, instancedSerializer);
            }

            return _dictionary;
        }
    }
}