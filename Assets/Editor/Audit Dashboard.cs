using System;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuditDashboard : EditorWindow
{

    //Tools/audit/Scan Active Scene
    //Tools/Audit/Scan All Scene

    bool checkLaming;
    bool checkLayer;
    bool checkStatic;
    bool checkMissingComponents;

    List<AudiTag> audiTags = new List<AudiTag>();
    AudiTag selectedAudiTag;

    [MenuItem("Window/Project/Audit Dashboard")]
    public static void ShowWindow()
    {
        GetWindow<AuditDashboard>("Audit Dashboard");
    }
    private void OnEnable()
    {
        //audiTags = new List<AudiTag>();
        audiTags.Clear();
        selectedAudiTag = null;

        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        //audiTags = new List<AudiTag>();
        audiTags.Clear();
        selectedAudiTag = null;

        SceneView.duringSceneGui -= OnSceneGUI;
    }
    private void OnSceneGUI(SceneView view)
    {
        //if (selectedAudiTag != null)
        //{
        //    Selection.activeObject = selectedAudiTag;
        //}
    }
    private void OnGUI()
    {
        checkLaming = EditorGUILayout.Toggle("Check Naming", checkLaming);
        checkLayer = EditorGUILayout.Toggle("Check Layer", checkLayer);
        checkStatic = EditorGUILayout.Toggle("Check Static", checkStatic);
        checkMissingComponents = EditorGUILayout.Toggle("Check Missing Components", checkMissingComponents);

        if (GUILayout.Button("Scan Scene"))
        {
            //scannerizzo la scena in cui mi trovo e tutti gli oggetti
            //presenti che hanno lo script audiTag(?)

            //for (int i = 0; i < SceneManager.sceneCount; i++)
            //{
            //    GameObject[] rootObjs = SceneManager.GetSceneAt(i).GetRootGameObjects();
            //    foreach (GameObject obj in rootObjs)
            //    {
            //        AudiTag a = obj.GetComponent<AudiTag>();
            //        if (a != null)
            //        {
            //            audiTags.Add(a);
            //        }
            //    }
            //}

            audiTags = new List<AudiTag>();
            foreach (GameObject g in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                AudiTag a = g.GetComponent<AudiTag>();
                if (a != null)
                {
                    audiTags.Add(a);
                }

            }
        }
        foreach (AudiTag a in audiTags)
        {
            //fa vedere l'icona dello stato
            //fa vedere il nome dell'oggetto
            // e il tipo di problema
            GUILayout.Label(a.name.ToString());
        }

        GUILayout.Label("Selected AudiTag: " + (selectedAudiTag != null ? selectedAudiTag : "Null"));

        //panel
        Event e = Event.current;

        float cellSize = 64f;
        float padding = 8f;

        Rect rectCanvas = GUILayoutUtility.GetRect(
            position.width,
            position.height - 80
        );

        EditorGUI.DrawRect(rectCanvas, new Color(0f, 0f, 0.12f));

        GUI.BeginGroup(rectCanvas);

        int cols = Mathf.FloorToInt((rectCanvas.width - padding) / (cellSize + padding));
        int index = 0;

        foreach (AudiTag _audiTag in audiTags)
        {
            int row = index / cols;
            int col = index % cols;
            Rect cellRect = new Rect(
                (padding + col * (cellSize + padding)),
                (padding + row * (cellSize + padding)) / 2,
                cellSize,
                cellSize / 2
            );

            EditorGUI.DrawRect(cellRect, new Color(0.3f, 0.3f, 0.35f));

            var preview = AssetPreview.GetAssetPreview(_audiTag);
            if (preview != null)
                GUI.DrawTexture(cellRect, preview, ScaleMode.ScaleToFit);
            else
                EditorGUI.LabelField(cellRect, _audiTag.name, EditorStyles.boldLabel);

            if (cellRect.Contains(e.mousePosition) && e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    selectedAudiTag = _audiTag;
                    if (selectedAudiTag != null)
                    {
                        Selection.activeObject = selectedAudiTag;

                        //SceneView sceneView = (SceneView)SceneView.sceneViews[0];
                        //sceneView.Focus();
                    }
                    e.Use();
                }
            }
            index++;
        }

        GUI.EndGroup();

        if (rectCanvas.Contains(e.mousePosition))
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        selectedAudiTag = null;
                        e.Use();
                    }
                    break;
            }
        }
    }
    private void OnSelectionChange()
    {
        Repaint();
    }
}
