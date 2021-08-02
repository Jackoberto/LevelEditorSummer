using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TilePlacing : MonoBehaviour, IEditorMode
{
    public GameObject mouseTile, tilesMaster, defaultImage;
    public Text currentMapNum;
    public TileList availableTiles;
    public Button incrementCurrentMapButton;
    public List<GameObject> tileGameObjects = new List<GameObject>();
    private Editor editor;
    private Grid grid;
    private Tilemap[] maps;
    private Tilemap map;
    private int tileNum;
    private Vector3Int lastPosition;
    private int currentMap;
    private SpriteRenderer rend;

    public string Name => "Tile Placing";

    public void Initialize(Editor editor)
    {
	    this.editor = editor;
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
        grid = GetComponentInChildren<Grid>();
        maps = GetComponentsInChildren<Tilemap>();
        rend = mouseTile.GetComponent<SpriteRenderer>();
        incrementCurrentMapButton.onClick.AddListener(IncrementCurrentMap);
    }

    public void Enter()
    {
        editor.ToggleAll(tileGameObjects);
    }

    public void Exit()
    {
        editor.ToggleAll(tileGameObjects);
    }

    public void EditorUpdate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
		var position = grid.WorldToCell(worldPoint);
        var overUI = editor.IsPointerOverUI();
        var tilePlacingMode = editor.ValidMousePosition(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        map = maps[currentMap];

		if (tilePlacingMode)
		{

			if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftShift) && !overUI)
			{
				var xDiff = position.x - lastPosition.x;
				var yDiff = position.y - lastPosition.y;
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
				editor.IncrementValue(ref tileNum, availableTiles.Tiles.Length);
			}

			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				editor.DecrementValue(ref tileNum, availableTiles.Tiles.Length);
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

    private void ChooseTile(int chosenTile)
    {
        tileNum = chosenTile;
    }
    
    private void IncrementCurrentMap()
    {
	    editor.IncrementValue(ref currentMap, maps.Length);
	    currentMapNum.text = currentMap.ToString();
    }
}