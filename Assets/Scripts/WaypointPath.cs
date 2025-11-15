using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> waypoints = new List<Transform>();

    [HideInInspector]
    public float totalLength;
    [HideInInspector]
    public bool lengthComputed = false;

    LineRenderer lineRenderer;

    private void OnDisable()
    {
        lengthComputed = false;
    }

    public void CollectFromChildren()
    {
        waypoints.Clear();
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }
    }

    public void SortByX()
    {
        waypoints.Sort((a, b) => a.position.x.CompareTo(b.position.x));
    }

    public void ComputeLength()
    {
        totalLength = 0f;

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            totalLength += Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
        }

        lengthComputed = true;
    }

    public void BakeToLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        else
            lineRenderer.enabled = true;

        lineRenderer.positionCount = waypoints.Count;
        for (int i = 0; i < waypoints.Count; i++)
        {
            lineRenderer.SetPosition(i, waypoints[i].position);
        }
    }

    public void HideLineRenderer()
    {
        if(lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }
}
