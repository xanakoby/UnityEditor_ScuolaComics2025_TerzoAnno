using System.IO;
using UnityEditor;
using UnityEngine;

public class CharacterManager : EditorWindow
{
    //int toolbaridx = 0;
    //string[] tabs = { "home", "edit", "view" };

    //int grididx = 0;
    //string[] grid = { "uno", "due", "tre", "quatro", "cinque", };

    GameObject targetObject;

    string characterName = "New Character";
    int age = 18;
    bool isActive = true;
    Color characterColor = Color.white;
    Vector3 position = Vector3.zero;

    [MenuItem("Window/Character Manager")]
    public static void ShowWindow()
    {
        GetWindow<CharacterManager>("Character Manager");
    }

    private void OnGUI()
    {
        //toolbaridx = GUILayout.Toolbar(toolbaridx, tabs);
        //grididx = GUILayout.SelectionGrid(grididx, grid, 3);

        GUILayout.BeginVertical("box");

        GUILayout.Label("Character personalization", EditorStyles.boldLabel);

        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        characterName = EditorGUILayout.TextField("Name", characterName);

        //GUILayout.BeginHorizontal();

        age = EditorGUILayout.IntSlider("Age", age, 0, 100);

        if (age < 18)
            EditorGUILayout.HelpBox("The age is < 18", MessageType.Warning);

        //GUILayout.EndHorizontal();

        isActive = EditorGUILayout.Toggle("Active", isActive);

        characterColor = EditorGUILayout.ColorField("Color", characterColor);

        position = EditorGUILayout.Vector3Field("Position", position);

        if (GUILayout.Button("Print data"))
        {
            string activeStatus = isActive ? "active" : "inactive";

            Debug.Log($"Name: {characterName}" +
                $"\nAge: {age}\nActive: {isActive}" +
                $"\nand it's: {isActive}" +
                $"\nColor: {characterColor}" +
                $"\nPosition: {position}");
        }

        if (GUILayout.Button("Modify Object"))
        {
            ApplyChanges();
        }

        GUILayout.EndVertical();
    }

    private void ApplyChanges()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("No object selected");
            return;
        }

        bool isPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(targetObject);
        if (isPrefabAsset)
        {
            string path = AssetDatabase.GetAssetPath(targetObject);
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);
            if (prefabRoot != null)
            {
                ChangeObjectProprieties(targetObject);

                PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
                PrefabUtility.UnloadPrefabContents(prefabRoot);

                Debug.Log($"Prefab {targetObject.name} modified");
            }
        }
        else
        {
            ChangeObjectProprieties(targetObject);

            EditorUtility.SetDirty(targetObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(targetObject.scene);

            Debug.Log($"Object {targetObject.name} modified");
        }
    }
    private void ChangeObjectProprieties(GameObject obj)
    {
        targetObject.name = characterName;
        targetObject.SetActive(isActive);
        targetObject.transform.position = position;

        Renderer rend = targetObject.GetComponent<Renderer>();
        if (rend != null)
        {
            Material mat = new Material(rend.sharedMaterial);
            mat.color = characterColor;
            rend.material = mat;
        }
    }
}
