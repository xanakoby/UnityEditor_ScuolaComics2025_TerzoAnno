using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelObjectManager : EditorWindow
{
    string objectName = "bye bye";
    bool isCube = true;
    float objectSize = 1f;

    List<GameObject> droppedObj = new List<GameObject>();
    GameObject selectedObj;

    bool autoRefresh = true;
    double lastRefresh;

    private void OnEnable()
    {
        autoRefresh = EditorPrefs.GetBool("LOM_AutoRefresh", true);

        Debug.Log("Open Level Object Manager Window");
    }
    private void OnDisable()
    {
        EditorPrefs.SetBool("LOM_AutoRefresh", autoRefresh);

        Debug.Log("Close Level Object Manager Window");
    }
    private void Update()
    {
        //ciclo di vita
        if (autoRefresh && EditorApplication.timeSinceStartup - lastRefresh > 0.25f)
        {
            lastRefresh = EditorApplication.timeSinceStartup;
            Repaint();
        }
    }

    [MenuItem("Window/Level Object Manager %l")]
    private static void ShowWindow()
    {
        var window = GetWindow<LevelObjectManager>();
        window.titleContent = new GUIContent("Level Object Manager");
        window.Show();
    }
    private void OnGUI()
    {
        GUILayout.Label("Life Cycle Options");
        autoRefresh = EditorGUILayout.Toggle("Auto Refresh", autoRefresh);
        GUILayout.Space(10);

        GUILayout.Label("Esempio di finestra", EditorStyles.boldLabel);
        objectName = EditorGUILayout.TextField("Object Name", objectName);
        isCube = EditorGUILayout.Toggle("Is Cube", isCube);
        objectSize = EditorGUILayout.Slider("Object Size", objectSize, 1, 2);

        if (GUILayout.Button("SpawnObject"))
        {
            //creo un oggetto nuovo di tipo cube o sphere
            GameObject obj = GameObject.CreatePrimitive(isCube ? PrimitiveType.Cube : PrimitiveType.Sphere);
            obj.name = objectName;
            obj.transform.localScale = Vector3.one * objectSize;
        }
        if (GUILayout.Button("Clear Selected"))
        {
            droppedObj.Clear();
        }

        Event e = Event.current;
        var rectCanvas = GUILayoutUtility.GetRect(position.width, position.height - 60);

        EditorGUI.DrawRect(rectCanvas, new Color(0f, 0f, 0.12f));

        GUI.BeginGroup(rectCanvas);

        float cellSize = 32f;
        float padding = 5f;
        int cols = Mathf.FloorToInt((rectCanvas.width - padding) / (cellSize + padding));
        int index = 0;

        foreach (var obj in droppedObj)
        {
            int row = index / cols;
            int col = index % cols;
            Rect cellRect = new Rect(
                (padding + col * (cellSize + padding))* 2,
                padding + row * (cellSize + padding),
                cellSize * 2,
                cellSize
            );

            EditorGUI.DrawRect(cellRect, new Color(0.3f, 0.3f, 0.35f));

            var preview = AssetPreview.GetAssetPreview(obj);
            if (preview != null)
                GUI.DrawTexture(cellRect, preview, ScaleMode.ScaleToFit);
            else
                EditorGUI.LabelField(cellRect, obj.name, EditorStyles.boldLabel);

            if (cellRect.Contains(e.mousePosition) && e.type == EventType.MouseDown)
            {
                if (e.button == 1)
                {
                    selectedObj = obj;
                    //Repaint();

                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Print Detail Name"), false, () => Debug.Log("name: "+ selectedObj));
                    menu.AddItem(new GUIContent("Print Detail Pos"), false, () => Debug.Log("Pos: "+ selectedObj.transform.position));
                    menu.ShowAsContext();
                    e.Use();
                }
            }

            //if (obj == selectedObj)
            //    Handles.DrawSolidRectangleWithOutline(cellRect, Color.clear, Color.red);

            index++;
        }

        GUI.EndGroup();


        foreach (var obj in droppedObj)
        {
            if (GUILayout.Button("SpawnObject"))
            {
                selectedObj = obj;
            }
        }

        if (rectCanvas.Contains(e.mousePosition))
        {
            switch (e.type)
            {
                case EventType.DragUpdated:
                    bool valid = DragAndDrop.objectReferences != null
                        && DragAndDrop.objectReferences.Length > 0
                        && DragAndDrop.objectReferences.All(o => o is GameObject); //Texture2D

                    DragAndDrop.visualMode = valid ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
                    e.Use();
                    break;
                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    foreach (var obj in DragAndDrop.objectReferences)
                    {
                        if (obj is GameObject o)
                        {
                            droppedObj.Add(o);
                            Debug.Log("Dropped object: " + o.name);
                        }
                    }
                    e.Use();
                    break;

                case EventType.DragExited:
                    //
                    break;

                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        Debug.Log("Click su" + e.mousePosition);
                        e.Use();
                    }
                    else if (e.button == 1)
                    {
                        //var menu = new GenericMenu();
                        //menu.AddItem(new GUIContent("Azione 1"), false, () => Debug.Log("Azione 1"));
                        //menu.AddItem(new GUIContent("Azione 2"), false, () => Debug.Log("Azione 2"));
                        //menu.ShowAsContext();
                        //e.Use();
                    }
                    break;

                case EventType.MouseDrag:
                    e.Use();
                    break;

                case EventType.ScrollWheel:
                    e.Use();
                    break;
            }
        }

    }
    private void OnFocus()
    {
        Debug.Log(" Got focus on Level Object Manager window");
    }
    private void OnLostFocus()
    {
        Debug.Log(" Lost focus on Level Object Manager window");
    }
}
