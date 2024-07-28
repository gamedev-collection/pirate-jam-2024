using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShadowDirection
{
    None,
    North,
    East,
    South,
    West
}

public class Obelisk : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _shadowDistance = 3;
    [SerializeField] private LayerMask _raycastLayers;
    [SerializeField] private ShadowDirection _shadowDirection = ShadowDirection.North;
    [SerializeField] private Rune _runeSlot;

    [Header("Affected Paths")]
    [SerializeField] private List<PathNodeWithDir> _pathNodesNorth = new List<PathNodeWithDir>();
    [SerializeField] private List<PathNodeWithDir> _pathNodesEast = new List<PathNodeWithDir>();
    [SerializeField] private List<PathNodeWithDir> _pathNodesSouth = new List<PathNodeWithDir>();
    [SerializeField] private List<PathNodeWithDir> _pathNodesWest = new List<PathNodeWithDir>();

    [Header("Shadow Objects - N E S W")]
    [SerializeField] private GameObject[] _shadowVisuals;

    public int ShadowDistance { get { return _shadowDistance; } set { _shadowDistance = value; } }
    public Rune RuneSlot { get { return _runeSlot; } }

    private GameObject _runeVisual;
    private Vector2 _shadowVector = Vector2.up;
    private List<Tower> _buffedTowers = new List<Tower>();

    private bool isHovered = false;

    private void OnEnable()
    {
        SetShadowDirection(_shadowDirection);
    }

    private void OnMouseEnter()
    {
        isHovered = true;
    }

    private void OnMouseExit()
    {
        isHovered = false;
    }

    private void Update()
    {
        if (!isHovered) return;

        if (Input.GetMouseButtonDown(1))
        {
            RemoveRune();
        }
    }

    public void SetNextShadowDirection()
    {
        SetShadowDirection(GetNextShadowDirection());
    }

    public void SetPrevShadowDirection()
    {
        SetShadowDirection(GetPrevShadowDirection());
    }

    public void SetShadowDirection(ShadowDirection direction)
    {
        DebuffTowers();
        _shadowDirection = direction;

        foreach (GameObject obj in _shadowVisuals)
        {
            if (obj != null) obj.SetActive(false);
        }

        switch (direction)
        {
            case ShadowDirection.North:
                _shadowVector = Vector2.up;
                _shadowVisuals[0].SetActive(true);
                break;
            case ShadowDirection.East:
                _shadowVector = Vector2.right;
                _shadowVisuals[1].SetActive(true);
                break;
            case ShadowDirection.South:
                _shadowVector = Vector2.down;
                _shadowVisuals[2].SetActive(true);
                break;
            case ShadowDirection.West:
                _shadowVector = Vector2.left;
                _shadowVisuals[3].SetActive(true);
                break;
            case ShadowDirection.None:
                break;
        }

        if (_shadowDirection != ShadowDirection.None) FindShadowedObjects(); LockShadowedPaths();
    }

    public void FindShadowedObjects()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, _shadowVector, _shadowDistance, _raycastLayers);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.TryGetComponent<Tower>(out Tower hitTower))
            {
                if (!_buffedTowers.Contains(hitTower)) _buffedTowers.Add(hitTower);
            }
        }

        if (_runeSlot != null) BuffTowers();
    }

    public void ApplyRune(Rune rune)
    {
        _runeSlot = rune;
        _runeVisual = rune.gameObject;
        _runeVisual.transform.position = transform.position;
        _runeVisual.transform.parent = transform;
    }

    public void RemoveRune()
    {
        UIManager.Instance.money += Mathf.RoundToInt(_runeSlot.cost / 2f);
        
        Destroy(_runeVisual);
        _runeSlot = null;
        _runeVisual = null;

        DebuffTowers();
    }

    private void BuffTowers()
    {
        foreach (Tower tower in _buffedTowers)
        {
            tower.ApplyRune(_runeSlot);
        }
    }

    private void DebuffTowers()
    {
        foreach (Tower tower in _buffedTowers)
        {
            tower.RemoveRune(_runeSlot);
        }
        _buffedTowers.Clear();
    }

    private void LockShadowedPaths()
    {
        Debug.Log("Locking " + _shadowDirection);
        switch (_shadowDirection)
        {
            case ShadowDirection.North:
                foreach (PathNodeWithDir path in _pathNodesNorth) path._pathNode.DisableWalkable(path._direction);
                //foreach (PathNodeWithDir path in _pathNodesEast) path._pathNode.EnableWalkable(path._direction);
                //foreach (PathNodeWithDir path in _pathNodesSouth) path._pathNode.EnableWalkable(path._direction);
                //foreach (PathNodeWithDir path in _pathNodesWest) path._pathNode.EnableWalkable(path._direction);
                break;
            case ShadowDirection.East:
                //foreach (PathNodeWithDir path in _pathNodesNorth) path._pathNode.EnableWalkable(path._direction);
                foreach (PathNodeWithDir path in _pathNodesEast) path._pathNode.DisableWalkable(path._direction);
                //foreach (PathNodeWithDir path in _pathNodesSouth) path._pathNode.EnableWalkable(path._direction);
                //foreach (PathNodeWithDir path in _pathNodesWest) path._pathNode.EnableWalkable(path._direction);
                break;
            case ShadowDirection.South:
                //foreach (PathNodeWithDir path in _pathNodesNorth) path._pathNode.EnableWalkable(path._direction);
                //foreach (PathNodeWithDir path in _pathNodesEast) path._pathNode.EnableWalkable(path._direction);
                foreach (PathNodeWithDir path in _pathNodesSouth) path._pathNode.DisableWalkable(path._direction);
                //foreach (PathNodeWithDir path in _pathNodesWest) path._pathNode.EnableWalkable(path._direction);
                break;
            case ShadowDirection.West:
                //foreach (PathNodeWithDir path in _pathNodesNorth) path._pathNode.EnableWalkable(path._direction);
                //foreach (PathNodeWithDir path in _pathNodesEast) path._pathNode.EnableWalkable(path._direction);
                //foreach (PathNodeWithDir path in _pathNodesSouth) path._pathNode.EnableWalkable(path._direction);
                foreach (PathNodeWithDir path in _pathNodesWest) path._pathNode.DisableWalkable(path._direction);
                break;
            case ShadowDirection.None:
                break;
        }
    }

    public void UnlockPaths()
    {
        foreach (PathNodeWithDir path in _pathNodesNorth) path._pathNode.EnableWalkable(path._direction);
        foreach (PathNodeWithDir path in _pathNodesEast) path._pathNode.EnableWalkable(path._direction);
        foreach (PathNodeWithDir path in _pathNodesSouth) path._pathNode.EnableWalkable(path._direction);
        foreach (PathNodeWithDir path in _pathNodesWest) path._pathNode.EnableWalkable(path._direction);
    }

    private ShadowDirection GetNextShadowDirection()
    {
        ShadowDirection newDirection = _shadowDirection;

        switch (_shadowDirection)
        {
            case ShadowDirection.North: newDirection = ShadowDirection.East; break;
            case ShadowDirection.East: newDirection = ShadowDirection.South; break;
            case ShadowDirection.South: newDirection = ShadowDirection.West; break;
            case ShadowDirection.West: newDirection = ShadowDirection.North; break;
        }

        return newDirection;
    }

    private ShadowDirection GetPrevShadowDirection()
    {
        ShadowDirection newDirection = _shadowDirection;

        switch (_shadowDirection)
        {
            case ShadowDirection.North: newDirection = ShadowDirection.West; break;
            case ShadowDirection.West: newDirection = ShadowDirection.South; break;
            case ShadowDirection.South: newDirection = ShadowDirection.East; break;
            case ShadowDirection.East: newDirection = ShadowDirection.North; break;
        }

        return newDirection;
    }
}

[Serializable]
public struct PathNodeWithDir
{
    public PathNode _pathNode;
    public int _direction;
}
