using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Editor : MonoBehaviour {
	
	public TileList availableTiles;
	public EditorPrefabList editorPrefabs;
    public GameObject mouseTile, defaultImage, tilesMaster, prefabMaster;
    public Text currentMapNum;
    public Button incrementCurrentMapButton;
    public Text editorModeLabel;
    public GameObject[] tileGameObjects;
    public GameObject[] prefabGameObjects;
	private SpriteRenderer rend;
	private Grid grid;
	private Tilemap[] maps;
	private Tilemap map;
	private int tileNum;
	private Vector3Int lastPosition;
	private int currentMap;
	private EditorMode editorMode;

	public EditorMode EditorMode => editorMode;

	public void IncrementEditorMode()
	{
		switch (editorMode)
		{
			case EditorMode.TilePlacing:
			{
				editorMode = EditorMode.PrefabPlacing;
				ToggleAll(tileGameObjects);
                break;
			}
			case EditorMode.PrefabPlacing:
			{
				editorMode = EditorMode.TilePlacing;
				ToggleAll(prefabGameObjects);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
		}
		switch (editorMode)
		{
			case EditorMode.TilePlacing:
				ToggleAll(tileGameObjects);
				break;
			case EditorMode.PrefabPlacing:
				ToggleAll(prefabGameObjects);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		editorModeLabel.text = editorMode.ToString();
	}

	private void ToggleAll(IEnumerable<GameObject> gameObjects)
	{
		foreach (var go in gameObjects)
		{
			go.SetActive(!go.activeSelf);
		}
	}

	private bool ValidMousePosition(Vector3 mousePosScreen)
	{
		return !(mousePosScreen.x > 1) && !(mousePosScreen.x < 0) && !(mousePosScreen.y > 1) && !(mousePosScreen.y < 0);
	}
	
	void Start ()
	{
		InitializeFields();
		InitButtons();
	}

	private void IncrementValue(ref int value, int max)
	{
		value++;
		if (value >= max)
		{
			value = 0;
		}
	}
	
	private void DecrementValue(ref int value, int max)
	{
		value--;
		if (value <= -1)
		{
			value = max - 1;
		}
	}

	private void IncrementCurrentMap()
	{
		IncrementValue(ref currentMap, maps.Length);
		currentMapNum.text = currentMap.ToString();
	}

	private void InitButtons()
	{
		for (var i = 0; i < availableTiles.Tiles.Length; i++)
		{
			var instance = Instantiate(defaultImage, tilesMaster.transform);
			var image = instance.GetComponent<Image>();
			var temp = i;
			instance.GetComponent<Button>().onClick.AddListener(() => { ChooseTile(temp); });
			instance.name = availableTiles[i].name;
			image.sprite = availableTiles[i].sprite;
			image.color = availableTiles[i].color;
		}
		ChooseTile(0);
		
		for (var i = 0; i < editorPrefabs.Prefabs.Length; i++)
		{
			var instance = Instantiate(defaultImage, prefabMaster.transform);
			var image = instance.GetComponent<Image>();
			var temp = i;
			instance.GetComponent<Button>().onClick.AddListener(() => { ChoosePrefab(temp); });
			instance.name = editorPrefabs[i].name;
			image.sprite = editorPrefabs[i].previewImage;
		}
	}

	private void ChoosePrefab(int temp)
	{
		throw new NotImplementedException();
	}

	private void InitializeFields()
	{
		editorModeLabel.text = editorMode.ToString();
		incrementCurrentMapButton.onClick.AddListener(IncrementCurrentMap);
		grid = GetComponentInChildren<Grid>();
		maps = GetComponentsInChildren<Tilemap>();
		rend = mouseTile.GetComponent<SpriteRenderer>();
	}
	
	void Update()
	{
		if (EditorMode == EditorMode.TilePlacing)
			TilePlacing();
    }

	private void TilePlacing()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
		var position = grid.WorldToCell(worldPoint);
        var overUI = IsPointerOverUI();
        var tilePlacingMode = ValidMousePosition(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        map = maps[currentMap];

		if (tilePlacingMode)
		{

			if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftShift) && !overUI)
			{
				var xDiff = position.x - lastPosition.x;
				var yDiff = position.y - lastPosition.y;
				Debug.Log($"xDiff: {xDiff}\n " + $"yDiff: {yDiff}");
				if (xDiff < 0 && yDiff < 0)
				{
					map.BoxFill(position, availableTiles[tileNum], position.x, position.y, lastPosition.x, lastPosition.y);
				}

				else if (xDiff > 0 && yDiff > 0)
				{
					map.BoxFill(position, availableTiles[tileNum], lastPosition.x, lastPosition.y, position.x, position.y);
				}

				else if (xDiff > 0 && yDiff < 0)
				{
					map.BoxFill(position, availableTiles[tileNum], lastPosition.x, position.y, position.x, lastPosition.y);
				}

				else if (xDiff < 0 && yDiff > 0)
				{
					map.BoxFill(position, availableTiles[tileNum], position.x, lastPosition.y, lastPosition.x, position.y);
				}

				else if (xDiff > 0 && yDiff == 0)
				{
					for (var i = 0; i < xDiff + 1; i++)
					{
						map.SetTile(new Vector3Int(lastPosition.x + i, lastPosition.y, lastPosition.z), availableTiles[tileNum]);
					}
				}

				else if (xDiff < 0 && yDiff == 0)
				{
					for (var i = 0; i > xDiff - 1; i--)
					{
						map.SetTile(new Vector3Int(lastPosition.x + i, lastPosition.y, lastPosition.z), availableTiles[tileNum]);
					}
				}

				if (yDiff > 0 && xDiff == 0)
				{
					for (var i = 0; i < yDiff + 1; i++)
					{
						map.SetTile(new Vector3Int(lastPosition.x, lastPosition.y + i, lastPosition.z), availableTiles[tileNum]);
					}
				}

				else if (yDiff < 0 && xDiff == 0)
				{
					for (var i = 0; i > yDiff - 1; i--)
					{
						map.SetTile(new Vector3Int(lastPosition.x, lastPosition.y + i, lastPosition.z), availableTiles[tileNum]);
					}
				}

				lastPosition = position;

			}


			else if (Input.GetKey(KeyCode.Mouse0) && !overUI)
			{
				map.SetTile(position, availableTiles[tileNum]);
				lastPosition = position;
			}

			if (Input.GetKey(KeyCode.Mouse1) && !overUI)
			{
				map.SetTile(position, null);
			}

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				IncrementValue(ref tileNum, availableTiles.Tiles.Length);
			}

			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				DecrementValue(ref tileNum, availableTiles.Tiles.Length);
			}

			if (!overUI)
			{
				mouseTile.transform.position = position + new Vector3(0.5f, 0.5f, 0);
				rend.enabled = true;
			}

			else
			{
				rend.enabled = false;
			}
		}
		rend.sprite = availableTiles[tileNum].sprite;
		var color = availableTiles[tileNum].color;
		color.a = 120/255f;
		rend.color = color;
	}

	private bool IsPointerOverUI()
    {
	    var eventSystem = EventSystem.current;
	    return eventSystem.IsPointerOverGameObject();
    }

	private void ChooseTile(int chosenTile)
    {
	    tileNum = chosenTile;
    }
}

public enum EditorMode
{
	TilePlacing,
	PrefabPlacing
}