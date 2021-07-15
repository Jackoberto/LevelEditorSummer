using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    public Button buttonPrefab;
    public Transform buttonParent;
    private SaveMenu saveMenu;
    private Saving saveSystem;
    public void Open(Saving saveSystem, SaveMenu saveMenu)
    {
        this.saveMenu = saveMenu;
        this.saveSystem = saveSystem;
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

    private void GenerateLevelButtons()
    {
        var maps = saveSystem.GetAllMapNames();
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
        saveSystem.LoadInEditor(mapName);
    }
}