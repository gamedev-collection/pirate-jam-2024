using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathGenerator : Singleton<PathGenerator>
{
    public Tilemap waypointTilemap;

    private List<Vector2Int> _waypointList = new List<Vector2Int>();
    private Vector2Int _startingPoint = Vector2Int.zero;
    private Vector2Int _endPoint = Vector2Int.down;

    private void Start()
    {
        GenerateWaypointList();
    }

    private void GenerateWaypointList()
    {
        if (waypointTilemap is null) return;

        var tiles = GetTiles();

        foreach (var (pos, tile) in tiles)
        {
            Debug.Log($"{pos}, {tile}");
        }
    }
    
    private Dictionary<Vector3, PathTile> GetTiles()
    {
        var tiles = new Dictionary<Vector3, PathTile>();

        foreach (var pos in waypointTilemap.cellBounds.allPositionsWithin)
        {
            if (!waypointTilemap.HasTile(pos)) continue;
            
            var tile = waypointTilemap.GetTile<PathTile>(pos);
            if (tile != null)
            {
                tiles[pos] = tile;
            }
        }

        return tiles;
    }
}
