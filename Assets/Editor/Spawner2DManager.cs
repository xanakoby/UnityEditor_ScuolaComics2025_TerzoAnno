using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Spawner2DManager : EditorWindow
{
    Spawner2D spawner2DPrefab;


    [MenuItem("Window/Spawner 2D Manager")]
    public static void ShowWindow()
    {
        GetWindow<Spawner2DManager>("Spawner 2D Manager");
    }
    private void OnGUI()
    {
        spawner2DPrefab = (Spawner2D)EditorGUILayout.ObjectField("Spawner 2D Prefab", spawner2DPrefab, typeof(Spawner2D), true);
        if (spawner2DPrefab == null)
        {
            EditorGUILayout.HelpBox("The Spawner 2D Prefab is Null", MessageType.Warning);
            return;
        }

        int newCount = EditorGUILayout.IntSlider("Spawn Count", spawner2DPrefab.count, 0, 100);
        spawner2DPrefab.count = newCount;

        float newRadius = EditorGUILayout.Slider("Spawner Radius", spawner2DPrefab.radius, 0, 100);
        if (Mathf.Abs(newRadius - spawner2DPrefab.radius) > 0.001f)
        {
            Undo.RecordObject(spawner2DPrefab, "Change Radius");
            spawner2DPrefab.radius = newRadius;

            SceneView.RepaintAll();
        }

        bool clearBeforeSpawn = EditorGUILayout.Toggle("Clear Before Spawn", spawner2DPrefab.clearBeforeSpawn);
        spawner2DPrefab.clearBeforeSpawn = clearBeforeSpawn;

        if (GUILayout.Button("SpawnNow"))
        {
            spawner2DPrefab.SpawnNow();
        }

        if (GUILayout.Button("Clear"))
        {
            spawner2DPrefab.Clear();
        }
    }
}
