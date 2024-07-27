using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathVisualiser : MonoBehaviour
{
    private LineRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<LineRenderer>();
    }

    public void VisualisePath(List<PathNode> path)
    {
        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("PathVisualizer: No path points provided.");
            return;
        }
        
        var pathPositions = new Vector3[path.Count];
        for (var i = 0; i < path.Count; i++)
        {
            pathPositions[i] = new Vector3(path[i].transform.position.x, path[i].transform.position.y, 0f);
        }

        _renderer ??= GetComponent<LineRenderer>();
        _renderer.positionCount = pathPositions.Length;
        _renderer.SetPositions(pathPositions);
    }

    public void EnablePathVisualiser()
    {
        _renderer.enabled = true;
    }
    
    public void DisablePathVisualiser()
    {
        _renderer.enabled = false;
    }
}