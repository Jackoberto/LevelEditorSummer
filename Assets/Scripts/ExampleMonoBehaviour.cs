using UnityEngine;

public class ExampleMonoBehaviour : MonoBehaviour
{ 
    [EditorProperty]
    public int someValue = 10;
    [EditorProperty]
    public int bounciness = 5;
    [EditorProperty]
    public int mass = 3;
    [EditorProperty]
    public string someStringValue = "I'm a string and now I can be serialized";
}