using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerManager : Singleton<TowerManager>
{
    public Tilemap placementMap;
    public bool InBuildmode { get; private set; }

    public OverlayTile overlayPrefab;
    public Transform overlayContainer;

    public event Action<Tower> OnTowerPlaced;
    
    private GameObject _towerPrefab;
    private Dictionary<Vector2Int, OverlayTile> Map { get; set; }

    private Dictionary<Vector3, Tower> _activeTowers = new Dictionary<Vector3, Tower>();
    
    private void Start()
    {
        if (placementMap is null)
        {
            Debug.LogWarning("Placement map is empty");
            return;
        }
        
        ConstructMapDict();
        placementMap.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!InBuildmode) return;
        
        var hit = GetFocusedOnTile();

        if (!hit.HasValue) return;
        var tile = hit.Value.collider.GetComponent<OverlayTile>();
                
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick(tile);
        }
    }

    public void SetActiveTower(GameObject towerPrefab)
    {
        InBuildmode = true;
        placementMap.gameObject.SetActive(true);

        _towerPrefab = towerPrefab;
    }

    private void HandleMouseClick(OverlayTile tile)
    {
        if (_activeTowers.ContainsKey(tile.transform.position)) return;

        var newTower = Instantiate(_towerPrefab, tile.transform.position, Quaternion.identity);
        var towerComp = newTower.GetComponent<Tower>();
        _activeTowers.Add(newTower.transform.position, towerComp);
        OnTowerPlaced?.Invoke(towerComp);

        InBuildmode = false;
        placementMap.gameObject.SetActive(false);
    }
    
    private static RaycastHit2D? GetFocusedOnTile()
    {
        var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var mousePos2d = new Vector2(mousePos.x, mousePos.y);

        var hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    private void ConstructMapDict()
    {
        Map = new Dictionary<Vector2Int, OverlayTile>();
        var bound = placementMap.cellBounds;
        var sortingOrder = placementMap.GetComponent<TilemapRenderer>().sortingOrder;

        for (var z = bound.max.z; z >= bound.min.z; z--)
        {
            for (var y = bound.min.y; y < bound.max.y; y++)
            {
                for (var x = bound.min.x; x < bound.max.x; x++)
                {
                    var cellPosition = new Vector3Int(x, y, z);
                    
                    if (!placementMap.HasTile(cellPosition)) continue;

                    var gridPosition = new Vector2Int(x, y);
                    
                    if (Map.ContainsKey(gridPosition)) continue;

                    var overlayTile = Instantiate(overlayPrefab, overlayContainer);
                    var cellWorldPosition = placementMap.GetCellCenterWorld(cellPosition);
                    overlayTile.transform.position =
                        new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.y);
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                    
                    Map.Add(gridPosition, overlayTile);
                }
            }
        }
    }
}
