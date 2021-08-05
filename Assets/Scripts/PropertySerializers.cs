using System;
using System.Collections.Generic;
using System.Linq;

public static class PropertySerializers
{
    private static Dictionary<Type, IEditorPropertySerializer> _dictionary;

    public static Dictionary<Type, IEditorPropertySerializer> Dictionary
    {
        get
        {
            if (_dictionary != null)
                return _dictionary;
            _dictionary = new Dictionary<Type, IEditorPropertySerializer>();
            var propertySerializers = System.Reflection.Assembly
                .GetAssembly(typeof(IEditorPropertySerializer))
                .GetTypes()
                .Where(type => typeof(IEditorPropertySerializer).IsAssignableFrom(type) && !type.IsAbstract);
            foreach (var propertySerializer in propertySerializers)
            {
                var constructor = propertySerializer.GetConstructor(Type.EmptyTypes);
                if (constructor == null)
                    continue;
                var instancedSerializer = (IEditorPropertySerializer) constructor.Invoke(Array.Empty<object>());
                var type = instancedSerializer.SerializedType;
                if (!_dictionary.ContainsKey(type))
                    _dictionary.Add(type, instancedSerializer);
            }

            return _dictionary;
        }
    }
}