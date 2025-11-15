using UnityEditor;
using UnityEngine;

public class WaypointPathManager : EditorWindow
{
    //bool updateAsSelected;
    private WaypointPath waypontPath;

    [MenuItem("Window/Waypoint Path Manager")]
    public static void ShowWindow()
    {
        GetWindow<WaypointPathManager>("Waypoint Path Manager");
    }

    private void OnEnable()
    {
        //if (!updateAsSelected)
        //    return;

        Selection.selectionChanged += OnSelectionChanged;
        OnSelectionChanged();
    }
    private void OnDisable()
    {
        //if (updateAsSelected)
        Selection.selectionChanged -= OnSelectionChanged;

        if (waypontPath != null)
        {
            waypontPath.lengthComputed = false;
            waypontPath.waypoints.Clear();
        }
    }

    private void OnGUI()
    {
        //updateAsSelected = EditorGUILayout.Toggle("update As Selected", updateAsSelected);
        //if(!updateAsSelected)
        waypontPath = (WaypointPath)EditorGUILayout.ObjectField("Waypoint Path", waypontPath, typeof(WaypointPath), true);
        if (waypontPath == null)
        {
            EditorGUILayout.HelpBox("The selected object is not a Spawner Waypont Path", MessageType.Warning);
            return;
        }
        if (waypontPath.waypoints.Count == 0)
        {
            EditorGUILayout.HelpBox("No waypoints in list.", MessageType.Info);
        }

        foreach (var wp in waypontPath.waypoints)
        {
            EditorGUILayout.ObjectField(wp, typeof(Transform), true);
        }

        if (GUILayout.Button("Collect From Children"))
        {
            waypontPath.CollectFromChildren();
        }

        if (GUILayout.Button("Sort By X"))
        {
            waypontPath.SortByX();
        }

        if (GUILayout.Button("Compute Length"))
        {
            waypontPath.ComputeLength();
        }

        if (waypontPath != null && waypontPath.lengthComputed)
            EditorGUILayout.LabelField("Total Length:", waypontPath.totalLength.ToString() + " units");

        if (GUILayout.Button("Bake To LineRenderer"))
        {
            waypontPath.BakeToLineRenderer();
        }
        if (GUILayout.Button("Hide LineRenderer"))
        {
            waypontPath.HideLineRenderer();
        }
    }

    private void OnSelectionChanged()
    {
        if (Selection.activeGameObject != null)
        {
            WaypointPath _waypointPath = Selection.activeGameObject.GetComponent<WaypointPath>();
            if (_waypointPath != null)
            {
                waypontPath = _waypointPath;
                Repaint();
            }
            else
            {
                waypontPath = null;
                Repaint();
            }
        }
        else
        {
            waypontPath = null;
            Repaint();
        }
    }
}
