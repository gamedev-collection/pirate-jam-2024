using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathManager : Singleton<PathManager>
{
    public PathNode startNode;

    public List<List<PathNode>> AllPaths { get; private set; }
    private readonly Dictionary<PathNode, List<PathNode>> _adjacencyList = new Dictionary<PathNode, List<PathNode>>();

    private void Start()
    {
        GenerateAllPaths();
        
        PrintPaths();
    }

    public void GenerateAllPaths()
    {
        BuildAdjacencyList();
        AllPaths = new List<List<PathNode>>();
        var currentPath = new List<PathNode>();
        var visitedNodes = new HashSet<PathNode>();
        DFS(startNode, currentPath, visitedNodes);
    }
    
    private void DFS(PathNode currentNode, List<PathNode> currentPath, HashSet<PathNode> visitedNodes)
    {
        if (currentNode == null || visitedNodes.Contains(currentNode))
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
            foreach (var nextNode in _adjacencyList[currentNode])
            {
                DFS(nextNode, currentPath, visitedNodes);
            }
        }

        // Backtrack
        currentPath.RemoveAt(currentPath.Count - 1);
        visitedNodes.Remove(currentNode);
    }
    
    private void BuildAdjacencyList()
    {
        _adjacencyList.Clear();
        var allNodes = FindObjectsOfType<PathNode>();
        foreach (var node in allNodes)
        {
            if (!_adjacencyList.ContainsKey(node))
            {
                _adjacencyList[node] = new List<PathNode>();
            }

            foreach (var path in node.nextPaths.Where(path => path.isWalkable && path.nextNode != null))
            {
                _adjacencyList[node].Add(path.nextNode);

                if (!_adjacencyList.ContainsKey(path.nextNode))
                {
                    _adjacencyList[path.nextNode] = new List<PathNode>();
                }

                _adjacencyList[path.nextNode].Add(node); // Make it bidirectional
            }
        }
    }

    private void PrintPaths()
    {
        foreach (var pathString in AllPaths.Select(path => path.Aggregate("", (current, node) => current + (node.name + " -> "))))
        {
            Debug.Log("Path: " + pathString.TrimEnd(' ', '-', '>'));
        }
    }
}