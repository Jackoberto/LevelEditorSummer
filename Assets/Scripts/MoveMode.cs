using UnityEngine;

public class MoveMode : MonoBehaviour, IEditorMode
{
    private PreviousPoint previousPoint;
    private Camera mainCamera;
    private Editor editor;
    public string Name => "Move Tool";

    public void Initialize(Editor editor)
    {
        mainCamera = Camera.main;
        this.editor = editor;
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
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
            var hitInfo = Physics2D.Raycast(rayOrigin,Vector2.zero);
            if (hitInfo.transform != null && hitInfo.transform.gameObject.TryGetComponent<EditorPrefabVisual>(out var prefabVisual))
            {
                previousPoint = new PreviousPoint(worldPoint, hitInfo.transform);
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
}