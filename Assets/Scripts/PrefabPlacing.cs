﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PrefabPlacing : MonoBehaviour, IEditorMode
{
    public EditorPrefabList editorPrefabs;
    public GameObject defaultImage, prefabMaster;
    public List<GameObject> prefabGameObjects = new List<GameObject>();
    private Editor editor;
    private int prefabNum;
    private EditorPrefabVisual currentPrefab;

    public string Name => "PrefabPlacing";
    
    private int PrefabNum
    {
        get => prefabNum;
        set
        {
            prefabNum = value;
            if (prefabNum >= editorPrefabs.Prefabs.Length)
                prefabNum = 0;
            if (currentPrefab != null)
            {
                prefabGameObjects.Remove(currentPrefab.gameObject);
                Destroy(currentPrefab);
            }
            currentPrefab = Instantiate(editorPrefabs[prefabNum].editorPrefabVisual);
            foreach (var monoBehaviour in currentPrefab.GetComponentsInChildren<MonoBehaviour>())
            {
                monoBehaviour.enabled = false;
            }
            prefabGameObjects.Add(currentPrefab.gameObject);
        }
    }
    
    public void Initialize(Editor editor)
    {
        this.editor = editor;
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

    public void Enter()
    {
        editor.ToggleAll(prefabGameObjects);
        PrefabNum = PrefabNum;
    }

    public void Exit()
    {
        editor.ToggleAll(prefabGameObjects);
    }

    public void EditorUpdate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        var overUI = editor.IsPointerOverUI();
        var placingMode = editor.ValidMousePosition(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        if (placingMode)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !overUI)
            {
                var instance = Instantiate(currentPrefab, worldPoint,
                    editorPrefabs[PrefabNum].editorPrefabVisual.transform.rotation);
                instance.EditorPrefab = editorPrefabs[PrefabNum];
            }
        }
        currentPrefab.transform.position = worldPoint;
    }
    
    private void ChoosePrefab(int temp)
    {
        PrefabNum = temp;
    }
}