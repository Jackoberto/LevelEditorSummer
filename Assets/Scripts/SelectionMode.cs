using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMode : MonoBehaviour, IEditorMode
{
    public Transform propertyParent;
    public PropertyInput propertyInput;
    public GameObject noProperties;
    public Text currentObjectLabel;
    public Text deleteLabel;
    public List<GameObject> prefabGameObjects = new List<GameObject>();
    private PreviousPoint previousPoint;
    private Editor editor;
    private Camera mainCamera;
    private EditorPrefabVisual prefabVisual;
    public string Name => "Selection/Move Tool";
    public void Initialize(Editor editor)
    {
        mainCamera = Camera.main;
        this.editor = editor;
    }

    public void Enter()
    {
        editor.ToggleAll(prefabGameObjects);
    }

    public void Exit()
    {
        editor.ToggleAll(prefabGameObjects);
    }

    public void EditorUpdate()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = mainCamera.transform.position.z * -1;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        var overUI = editor.IsPointerOverUI();
        var tilePlacingMode = editor.ValidMousePosition(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        if (overUI || !tilePlacingMode)
            return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var rayOrigin = mainCamera.ScreenToWorldPoint(mousePos);
            var hitInfo = Physics2D.Raycast(rayOrigin, Vector2.zero);
            if (hitInfo.transform != null &&
                hitInfo.transform.gameObject.TryGetComponent<EditorPrefabVisual>(out var prefabVisual))
            {
                currentObjectLabel.text = prefabVisual.EditorPrefab.DisplayName;
                deleteLabel.text = "Delete Instance";
                previousPoint = new PreviousPoint(worldPoint, hitInfo.transform);
                this.prefabVisual = prefabVisual;
                foreach (var child in propertyParent.GetComponentsInChildren<Transform>())
                {
                    if(child != propertyParent)
                        Destroy(child.gameObject);
                }
                foreach (var serializedProperty in prefabVisual.SerializedProperties)
                {
                    var instance = Instantiate(propertyInput, propertyParent);
                    instance.Setup(serializedProperty);
                }
                if (prefabVisual.SerializedProperties.Count == 0)
                {
                    Instantiate(noProperties, propertyParent);
                }
            }
        }
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (previousPoint != null)
            {
                var pointDelta = worldPoint - previousPoint.Point;
                previousPoint.Transform.position += pointDelta;
                previousPoint.Point = worldPoint;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            previousPoint = null;
        }
    }

    public void DeleteInstance()
    {
        if (prefabVisual != null)
        {
            Destroy(prefabVisual.gameObject);
            foreach (var child in propertyParent.GetComponentsInChildren<Transform>())
            {
                if(child != propertyParent)
                    Destroy(child.gameObject);
            }

            currentObjectLabel.text = "";
            deleteLabel.text = "";
        }
    }
}