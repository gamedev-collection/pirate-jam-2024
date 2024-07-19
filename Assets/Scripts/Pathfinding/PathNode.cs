using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public bool isStart;
    public bool isEnd;
    public List<Path> nextPaths = new List<Path>();

    public void EnableWalkable(int index)
    {
        SetWalkable(index, true);
    }
    
    public void DisableWalkable(int index)
    {
        SetWalkable(index, false);
    }
    
    private void SetWalkable(int index, bool isWalkable)
    {
        if (index < 0 || index > nextPaths.Count) return;

        var path = nextPaths[index];
        path.isWalkable = isWalkable;
        nextPaths[index] = path;
        
        PathManager.Instance.GenerateAllPaths();
    }
    
    private void OnDrawGizmos()
    {
        if (isStart)
        {
            Gizmos.color = Color.green;
        }

        if (isEnd)
        {
            Gizmos.color = Color.red;
        }

        if (!isStart && !isEnd)
        {
            Gizmos.color = Color.blue;
        }
        
        Gizmos.DrawCube(transform.position, new Vector3(0.3f,0.3f,0.3f));
        
        if (nextPaths.Count <= 0) return;
        foreach (var path in nextPaths.Where(path => path.nextNode is not null))
        {
            Gizmos.color = path.isWalkable ? Color.blue : Color.red;
            Gizmos.DrawLine(transform.position, path.nextNode.transform.position);
        }
    }
}

[Serializable]
public struct Path
{
    public bool isWalkable;
    public PathNode nextNode;
}
