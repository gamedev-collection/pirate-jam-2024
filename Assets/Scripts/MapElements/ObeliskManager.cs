using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Obelisk;

public class ObeliskManager : Singleton<ObeliskManager>
{
    [SerializeField] private Obelisk[] _obelisks;

    private void Awake()
    {
        TowerManager.Instance.OnTowerPlaced += OnNewTowerPlaced;
    }

    private void OnDestroy()
    {
        TowerManager.Instance.OnTowerPlaced -= OnNewTowerPlaced;
    }

    public void RotateShadowsToNext()
    {
        foreach (Obelisk obelisk in _obelisks)
        {
            obelisk.SetNextShadowDirection();
        }
    }

    public void RotateShadowsToPrev()
    {
        foreach (Obelisk obelisk in _obelisks)
        {
            obelisk.SetPrevShadowDirection();
        }
    }

    public void RotateShadows(ShadowDirection direction)
    {
        foreach (Obelisk obelisk in _obelisks) { obelisk.SetShadowDirection(direction); }
    }

    private void OnNewTowerPlaced(Tower tower)
    {
        foreach(Obelisk obelisk in _obelisks)
        {
            obelisk.FindShadowedObjects();
        }
    }
}
