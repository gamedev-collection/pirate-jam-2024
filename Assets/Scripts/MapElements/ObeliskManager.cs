using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Obelisk;

public class ObeliskManager : Singleton<ObeliskManager>
{
    [SerializeField] private Obelisk[] _obelisks;

    [Tooltip("Optional - Searches for Obelisks in Children of Parent. Overrides Array")]
    [SerializeField] private GameObject _ObeliskParent;

    [SerializeField] private LayerMask _obeliskLayerMask;
    [SerializeField] private float _delay;

    public List<PathNodeWithDir> DisabledPaths = new List<PathNodeWithDir>();

    private Rune _selectedRunePrefab;
    private GameObject _runePlacementVisual;

    public bool InRunePlacementMode { get; private set; }

    private void Awake()
    {
        TowerManager.Instance.OnTowerPlaced += OnNewTowerPlaced;
        if (_ObeliskParent)
        {
            _obelisks = _ObeliskParent.GetComponentsInChildren<Obelisk>();
        }
    }

    private void OnDestroy()
    {
        //TowerManager.Instance.OnTowerPlaced -= OnNewTowerPlaced;
    }

    private void Update()
    {
        if (!InRunePlacementMode) return;
        
        if (_runePlacementVisual)
        {
            Vector3 mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            _runePlacementVisual.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Obelisk clickedObelisk = GetClickedObelisk();
            if (clickedObelisk) HandleRunePlacement(clickedObelisk);
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelActiveRune(true);
        }
    }

    public void RotateToNextWithDelay()
    {
        StartCoroutine(RotateToNextWithDelay_Routine());
    }

    public IEnumerator RotateToNextWithDelay_Routine()
    {
        yield return new WaitForSeconds(_delay);
        RotateShadowsToNext();
    }

    public void RotateShadowsToNext()
    {
        UnlockAllPaths();
        foreach (Obelisk obelisk in _obelisks)
        {
            obelisk.SetNextShadowDirection();
        }
    }

    public void RotateShadowsToPrev()
    {
        UnlockAllPaths();
        foreach (Obelisk obelisk in _obelisks)
        {
            obelisk.SetPrevShadowDirection();
        }
    }

    public void RotateShadows(ShadowDirection direction)
    {
        UnlockAllPaths();
        foreach (Obelisk obelisk in _obelisks) { obelisk.SetShadowDirection(direction); }
    }

    public void UnlockAllPaths()
    {
        foreach (Obelisk obelisk in _obelisks) { obelisk.UnlockPaths(); }
    }


    private void OnNewTowerPlaced(Tower tower)
    {
        foreach (Obelisk obelisk in _obelisks)
        {
            obelisk.FindShadowedObjects();
        }
    }

    public void OnTowerSold(Tower tower)
    {
        foreach (Obelisk obelisk in _obelisks)
        {
            if (obelisk.BuffedTowers.Contains(tower))
            {
                obelisk.BuffedTowers.Remove(tower);
            }
        }
    }

    #region RUNES

    public void SetActiveRune(Rune runePrefab)
    {
        _selectedRunePrefab = runePrefab;
        _runePlacementVisual = Instantiate(_selectedRunePrefab, Vector3.zero, Quaternion.identity, this.transform).gameObject;
        _runePlacementVisual.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        InRunePlacementMode = true;
    }

    public void CancelActiveRune(bool deleteVisual)
    {
        _selectedRunePrefab = null;
        InRunePlacementMode = false;

        if (deleteVisual) Destroy(_runePlacementVisual);
        else _runePlacementVisual = null;
    }

    private Obelisk GetClickedObelisk()
    {
        var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var mousePos2d = new Vector2(mousePos.x, mousePos.y);

        var hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero, 15f, _obeliskLayerMask);
        if (hits.Length > 0)
        {
            var firstHit = hits.OrderByDescending(i => i.collider.transform.position.z).First();
            if (firstHit.transform.TryGetComponent<Obelisk>(out Obelisk clickedObelisk) && clickedObelisk.RuneSlot == null) return clickedObelisk;
        }

        

        return null;
    }

    private void HandleRunePlacement(Obelisk obelisk)
    {
        _runePlacementVisual.GetComponent<SpriteRenderer>().sortingLayerName = "Game";
        Rune rune = _runePlacementVisual.GetComponent<Rune>();
        obelisk.ApplyRune(rune);
        UIManager.Instance.money -= rune.cost;
        CancelActiveRune(false);
    }

    #endregion
}
