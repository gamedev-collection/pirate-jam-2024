using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBlocker : MonoBehaviour
{
    [SerializeField] private PathNodeWithDir[] _nodesNorth;
    [SerializeField] private PathNodeWithDir[] _nodesEast;
    [SerializeField] private PathNodeWithDir[] _nodesSouth;
    [SerializeField] private PathNodeWithDir[] _nodesWest;

    public void ClosePath(ShadowDirection shadowDirection)
    {
        foreach (var node in _nodesNorth) { node._pathNode.EnableWalkable(node._direction); }
        foreach (var node in _nodesEast) { node._pathNode.EnableWalkable(node._direction); }
        foreach (var node in _nodesSouth) { node._pathNode.EnableWalkable(node._direction); }
        foreach (var node in _nodesWest) { node._pathNode.EnableWalkable(node._direction); }

        switch (shadowDirection)
        {
            case ShadowDirection.North:
                foreach (var node in _nodesNorth) { node._pathNode.DisableWalkable(node._direction); }
                break;
            case ShadowDirection.East:
                foreach (var node in _nodesEast) { node._pathNode.DisableWalkable(node._direction); }
                break;
            case ShadowDirection.South:
                foreach (var node in _nodesSouth) { node._pathNode.DisableWalkable(node._direction); }
                break;
            case ShadowDirection.West:
                foreach (var node in _nodesWest) { node._pathNode.DisableWalkable(node._direction); }
                break;
        }
    }
}
