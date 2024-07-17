using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapExtensions
{
    public static T[] GetTiles<T>(this Tilemap tilemap) where T : TileBase
    {
        var tiles = new List<T>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;
            
            var tile = tilemap.GetTile<T>(pos);
            if (tile != null)
            {
                tiles.Add(tile);
            }
        }

        return tiles.ToArray();
    }
}
