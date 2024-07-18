using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public bool isStart;
    public bool isEnd;
    public bool isWalkable;
    public List<PathNode> nextNodes = new List<PathNode>();

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
        
        if (nextNodes.Count <= 0) return;

        Gizmos.color = Color.blue;
        
        foreach (var nextNode in nextNodes)
        {
            if (nextNode is null) return;
            Gizmos.DrawLine(transform.position, nextNode.transform.position);
        }
    }
}
