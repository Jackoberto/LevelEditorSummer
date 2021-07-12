using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Editor : MonoBehaviour {
	public Text editorModeLabel;
	private List<IEditorMode> editorModes = new List<IEditorMode>();
	private int currentEditorMode;
	
	public void IncrementEditorMode()
	{
		editorModes[currentEditorMode].Exit();
		IncrementValue(ref currentEditorMode, editorModes.Count);
		editorModes[currentEditorMode].Enter();
		editorModeLabel.text = editorModes[currentEditorMode].Name;
	}

	public void ToggleAll(IEnumerable<GameObject> gameObjects)
	{
		foreach (var go in gameObjects)
		{
			go.SetActive(!go.activeSelf);
		}
	}

	public bool ValidMousePosition(Vector3 mousePosScreen)
	{
		return !(mousePosScreen.x > 1) && !(mousePosScreen.x < 0) && !(mousePosScreen.y > 1) && !(mousePosScreen.y < 0);
	}
	
	void Start ()
	{
		InitializeFields();
	}

	public void IncrementValue(ref int value, int max)
	{
		value++;
		if (value >= max)
		{
			value = 0;
		}
	}

	public void DecrementValue(ref int value, int max)
	{
		value--;
		if (value <= -1)
		{
			value = max - 1;
		}
	}

	private void InitializeFields()
	{
		editorModes = GetComponents<IEditorMode>().ToList();
		foreach (var editorMode in editorModes)
		{
			editorMode.Initialize(this);	
		}
		editorModeLabel.text = editorModes[currentEditorMode].Name;
	}
	
	void Update()
	{
		editorModes[currentEditorMode].EditorUpdate();
    }

	public bool IsPointerOverUI()
    {
	    var eventSystem = EventSystem.current;
	    return eventSystem.IsPointerOverGameObject();
    }
}