using UnityEngine;
using UnityEngine.UI;

public class PropertyInput : MonoBehaviour
{
    public InputField inputField;
    public Text propertyLabel;
    private SerializedProperty serializedProperty;

    public void Setup(SerializedProperty serializedProperty)
    {
        this.serializedProperty = serializedProperty;
        propertyLabel.text = serializedProperty.name;
        inputField.SetTextWithoutNotify(serializedProperty.value);
    }

    public void TrySave()
    {
        if (PropertySerializers.Dictionary.TryGetValue(serializedProperty.Type, out var serializer))
        {
            Debug.Log(inputField.text);
            if (serializer.IsValid(inputField.text))
            {
                serializedProperty.value = inputField.text;
            }
        }
    }
}
