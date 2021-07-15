using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    public InputWindow savingMenu;
    public LoadMenu loadingMenu;
    private Saving saving;

    private void Start()
    {
        saving = FindObjectOfType<Saving>(true);
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OpenSavingMenu()
    {
        savingMenu.Open(saving.Save);
    }
    
    public void OpenLoadingMenu()
    {
        loadingMenu.Open(saving, this);
    }
}