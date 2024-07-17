using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New PathTile", menuName = "Tiles/PathTile")]
public class PathTile : RuleTile
{
    public bool walkable;
    public bool isStart;
    public bool isEnd;
}
