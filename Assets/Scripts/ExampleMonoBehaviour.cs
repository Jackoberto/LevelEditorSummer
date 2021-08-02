using UnityEngine;

public class ExampleMonoBehaviour : MonoBehaviour
{ 
    [EditorProperty]
    public int someValue = 10;
    [EditorProperty]
    public string someInvalidValue = "I'm a string but strings can't be serialized yet";
}