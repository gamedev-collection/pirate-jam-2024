using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathManager : Singleton<PathManager>
{
    public PathNode startNode;

    public List<List<PathNode>> AllPaths { get; private set; }

    private void Start()
    {
        FindAllPaths();

        foreach (var path in AllPaths)
        {
            if (path is null) continue;
            Debug.Log("Found Path");
            foreach (var pathNode in path)
            {
                Debug.Log($"Node: {pathNode.name}", pathNode.gameObject);
            }
        }
    }

    public void FindAllPaths()
    {
        AllPaths = new List<List<PathNode>>();
        var currentPath = new List<PathNode>();
        var visitedNodes = new HashSet<PathNode>();
        DFS(startNode, currentPath, visitedNodes);
    }
    
    private void DFS(PathNode currentNode, List<PathNode> currentPath, HashSet<PathNode> visitedNodes)
    {
        if (currentNode == null || !currentNode.isWalkable || visitedNodes.Contains(currentNode))
        {
            return;
        }

        currentPath.Add(currentNode);
        visitedNodes.Add(currentNode);

        if (currentNode.isEnd)
        {
            // Found a valid path
            AllPaths.Add(new List<PathNode>(currentPath));
        }
        else
        {
            // Explore all adjacent nodes
            foreach (var nextNode in currentNode.nextNodes)
            {
                DFS(nextNode, currentPath, visitedNodes);
            }
        }

        // Backtrack
        currentPath.RemoveAt(currentPath.Count - 1);
        visitedNodes.Remove(currentNode);
    }
}