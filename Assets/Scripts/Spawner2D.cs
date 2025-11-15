using System.Collections.Generic;
using UnityEngine;

public class Spawner2D : MonoBehaviour
{
    [Header("Setup")]
    public GameObject prefab;

    [Range(1, 100)]
    public int count = 10;

    [SerializeField] List<GameObject> spawnedObjects = new List<GameObject>();
    [Range(1, 100)]

    public float radius = 5f;

    [Header("Runtime")]
    public bool clearBeforeSpawn = true;

    public void SpawnNow()
    {
        if (prefab == null)
        {
            Debug.LogWarning("Prefab is null!");
            return;
        }

        if (clearBeforeSpawn)
            Clear();

        for (int i = 0; i < count; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float distance = Random.Range(0f, radius);
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
            Vector3 spawnPos = transform.position + offset;
            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
            spawnedObjects.Add(obj);
        }
    }

    public void Clear()
    {
        foreach(GameObject obj in spawnedObjects)
        {
            if (Application.isEditor)
                DestroyImmediate(obj);
            else
                Destroy(obj);
        }

        spawnedObjects.Clear();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
