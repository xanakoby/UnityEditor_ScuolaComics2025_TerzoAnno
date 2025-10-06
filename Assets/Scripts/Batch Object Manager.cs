using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static BatchObjectManager;

public class BatchObjectManager : EditorWindow
{
    public enum FilterType
    {
        Light,
        AudioSource
    }

    string renameObjectsText;
    FilterType filterType;

    [MenuItem("Window/Batch Object Manager")]
    private static void ShowWindow()
    {
        var window = GetWindow<BatchObjectManager>();
        window.titleContent = new GUIContent("Batch Object Manager");
        window.Show();
    }
    [MenuItem("CONTEXT/Transform/ResetY")]
    private static void ResetYPosSelectedObj()
    {
        Undo.RecordObjects(Selection.transforms, "Reset Y position");
        var objs = Selection.objects;
        foreach (var o in objs)
        {
            if (o != null)
            {
                o.GameObject().transform.position = new Vector3(o.GameObject().transform.position.x, 0, o.GameObject().transform.position.z);
            }
        }
    }
    private void OnSelectionChange()
    {
        var obj = Selection.activeObject;
        var objs = Selection.objects;

        //foreach (var o in objs)
        //{
        //    if (o != null)
        //        Debug.Log("Selezionati: " + o.name + " di tipo " + o.GetType());
        //    Debug.Log("Tra I Selezionati" + obj.name);
        //}
        //if (obj != null)
        //{
        //    Debug.Log("Selezionato: " + obj.name + " di tipo " + obj.GetType());
        //    Debug.Log("Attualmente selezionato" + obj.name);
        //}
        Repaint();
    }
    private void OnGUI()
    {
        GUILayout.Label("Selected object", EditorStyles.boldLabel);
        Object obj = Selection.activeObject;
        var objs = Selection.objects;
        foreach (var o in objs)
        {
            if (o != null)
                GUILayout.Label("Between selections: " + o.name + " of type " + o.GetType().Name.ToString());
        }
        if (obj != null)
        {
            GUILayout.Label("Last selected: " + obj.name);
            GUILayout.Label("Type: " + obj.GetType().Name.ToString());
        }
        else
        {
            GUILayout.Label("No object selected");
        }

        renameObjectsText = EditorGUILayout.TextField("Rename name:", renameObjectsText);
        if (GUILayout.Button("Rename selected objects"))
        {
            foreach (var o in objs)
            {
                if (o != null)
                {
                    Undo.RecordObject(o, "Reset name Obj to "+ o.name);
                    o.name = renameObjectsText;
                }
            }
        }

        filterType = (FilterType)EditorGUILayout.EnumPopup("Filter selected", filterType);
        if (GUILayout.Button("Filter selected objects"))
        {
            foreach (var o in objs)
            {
                if (o != null)
                {
                    switch(filterType)
                        {
                        case FilterType.Light:
                            if (o.GetComponent<Light>() != null)
                            {
                                Debug.Log(o.name + " has a Light");
                            }
                            break;
                        case FilterType.AudioSource:
                            if (o.GetComponent<AudioSource>() != null)
                            {
                                Debug.Log(o.name + " has a AudioSource");
                            }
                            break;
                    }
                    //if (o.GetComponent<Light>() != null)
                    //{
                    //    Debug.Log(o.name + " has a Light");
                    //}
                    //if (o.GetComponent<AudioSource>() != null)
                    //{
                    //    Debug.Log(o.name + " has a AudioSource");
                    //}
                }
            }
        }

        if (GUILayout.Button("Undo"))
        {
            Undo.PerformUndo();
        }
    }
}
