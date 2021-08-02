using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    public Button buttonPrefab;
    public Transform buttonParent;
    private SaveMenu saveMenu;
    private Saving saving;
    public void Open(Saving saving, SaveMenu saveMenu)
    {
        this.saveMenu = saveMenu;
        this.saving = saving;
        gameObject.SetActive(true);
        GenerateLevelButtons();
    }

    public void Close()
    {
        var children = buttonParent.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child != buttonParent)
                Destroy(child.gameObject);
        }
        saveMenu.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OpenSavesFolder()
    {
        saving.OpenSavesFolder();
    }
    
    public void Refresh()
    {
        var children = buttonParent.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child != buttonParent)
                Destroy(child.gameObject);
        }
        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        var maps = saving.GetAllMapNames();
        foreach (var map in maps)
        {
            var instance = Instantiate(buttonPrefab, buttonParent);
            instance.onClick.AddListener(() => LoadAndClose(map));
            instance.GetComponentInChildren<Text>().text = map;
        }
    }

    private void LoadAndClose(string mapName)
    {
        Close();
        saving.LoadInEditor(mapName);
    }
}