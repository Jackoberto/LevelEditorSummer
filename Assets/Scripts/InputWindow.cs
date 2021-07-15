using System;
using UnityEngine;
using UnityEngine.UI;

public class InputWindow : MonoBehaviour
{
    public InputField inputField;
    private event Action<string> ConfirmEvent;

    public void Open(Action<string> confirm)
    {
        gameObject.SetActive(true);
        ConfirmEvent += confirm;
    }

    public void Confirm()
    {
        gameObject.SetActive(false);
        ConfirmEvent?.Invoke(inputField.text);
        ConfirmEvent = null;
    }
    
    public void Cancel()
    {
        ConfirmEvent = null;
        gameObject.SetActive(false);
    }
}
