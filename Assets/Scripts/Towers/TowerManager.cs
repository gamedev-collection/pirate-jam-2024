using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class TowerManager : Singleton<TowerManager>
{
    public Tilemap placementMap;
    public bool InBuildmode { get; private set; }

    public OverlayTile overlayPrefab;
    public Transform overlayContainer;

    public GameObject rangeIndicator;
    
    public AudioClip placeAudioClip;
    public AudioClip removeAudioClip;

    public event Action<Tower> OnTowerPlaced;

    private GameObject _towerPrefab;
    private Dictionary<Vector2Int, OverlayTile> Map { get; set; }

    private Dictionary<Vector3, Tower> _activeTowers = new Dictionary<Vector3, Tower>();

    private GameObject _towerPlacementVisual;

    private AudioSource _audioSource;

    private void Start()
    {
        if (placementMap is null)
        {
            Debug.LogWarning("Placement map is empty");
            return;
        }

        ConstructMapDict();
        placementMap.gameObject.SetActive(false);

        if (rangeIndicator is null) return;
        rangeIndicator.SetActive(false);

        _audioSource ??= GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        if (!InBuildmode) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        if (_towerPlacementVisual)
        {
            _towerPlacementVisual.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
        rangeIndicator.transform.position = mousePos;

        var hit = GetFocusedOnTile();

        if (Input.GetMouseButtonDown(1))
        {
            CancelActiveTower(true);
        }

        if (!hit.HasValue) return;
        var tile = hit.Value.collider.GetComponent<OverlayTile>();

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick(tile);
            rangeIndicator.SetActive(false);
        }
    }

    public void RemoveActiveTower(Vector3 position)
    {
        _activeTowers.Remove(position);
        
        if (removeAudioClip) _audioSource?.PlayOneShot(removeAudioClip);
    }

    public void SetActiveTower(GameObject towerPrefab)
    {
        InBuildmode = true;
        placementMap.gameObject.SetActive(true);

        _towerPrefab = towerPrefab;
        var towerComponent = towerPrefab.GetComponent<Tower>();
        towerComponent.InBuildMode = InBuildmode;
        towerComponent.GetComponent<Collider2D>().enabled = false;

        rangeIndicator.GetComponent<SpriteOutline>().color = towerComponent.rangeIndicator.gameObject.GetComponent<SpriteOutline>().color;
        rangeIndicator.GetComponent<SpriteRenderer>().color = towerComponent.rangeIndicator.gameObject.GetComponent<SpriteRenderer>().color;
        rangeIndicator.SetActive(true);
        rangeIndicator.transform.localScale = new Vector2(towerComponent.range * 2, towerComponent.range * 2);
        _towerPlacementVisual = Instantiate(_towerPrefab, Vector3.zero, Quaternion.identity);
        _towerPlacementVisual.GetComponent<Tower>().visual.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
    }
    
    public void CancelActiveTower(bool deleteVisual)
    {
        InBuildmode = false;
        placementMap.gameObject.SetActive(false);
        rangeIndicator.SetActive(false);
        if (deleteVisual) Destroy(_towerPlacementVisual);
        else _towerPlacementVisual = null;
    }

    private void HandleMouseClick(OverlayTile tile)
    {
        if (tile is null) return;
            
        if (_activeTowers.ContainsKey(tile.transform.position)) return;

        _towerPlacementVisual.transform.position = tile.transform.position;
        _towerPlacementVisual.GetComponent<Tower>().visual.GetComponent<SpriteRenderer>().sortingLayerName = "Game";

        var towerComp = _towerPlacementVisual.GetComponent<Tower>();
        _activeTowers.Add(_towerPlacementVisual.transform.position, towerComp);

        OnTowerPlaced?.Invoke(towerComp);
        CancelActiveTower(false);
        towerComp.GetComponent<Collider2D>().enabled = true;
        towerComp.InBuildMode = InBuildmode;

        placementMap.gameObject.SetActive(false);
        UIManager.Instance.money -= _towerPrefab.GetComponent<Tower>().cost;
        
        if (placeAudioClip) _audioSource?.PlayOneShot(placeAudioClip);
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
