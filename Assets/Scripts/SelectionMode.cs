using System.Collections.Generic;
using UnityEngine;

public class SelectionMode : MonoBehaviour, IEditorMode
{
    public Transform propertyParent;
    public PropertyInput propertyInput;
    public GameObject noProperties;
    public List<GameObject> prefabGameObjects = new List<GameObject>();
    private Editor editor;
    private Camera mainCamera;
    public string Name => "Selection Tool";
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
        var overUI = editor.IsPointerOverUI();
        var tilePlacingMode = editor.ValidMousePosition(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        if (overUI || !tilePlacingMode)
            return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var mousePos = Input.mousePosition;
            mousePos.z = mainCamera.transform.position.z * -1;
            var rayOrigin = mainCamera.ScreenToWorldPoint(mousePos);
            var hitInfo = Physics2D.Raycast(rayOrigin, Vector2.zero);
            if (hitInfo.transform != null &&
                hitInfo.transform.gameObject.TryGetComponent<EditorPrefabVisual>(out var prefabVisual))
            {
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
    }
}