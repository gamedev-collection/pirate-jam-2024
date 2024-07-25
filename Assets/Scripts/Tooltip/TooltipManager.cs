using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TooltipManager : Singleton<TooltipManager>
{
    [SerializeField] private Tooltip _toolTip;
    [SerializeField] private TowerTooltip _towerToolTip;

    public Tooltip Tooltip { get { return _toolTip; } }
    public TowerTooltip TowerTooltip { get { return _towerToolTip; } }

    private Tooltip _activeTooltip;

    private void Awake()
    {
        Hide();
    }
    public static void ShowBasicTooltip(string body, string header = "")
    {
        if (Instance._activeTooltip) Hide();

        Instance._toolTip.SetText(body, header);
        Instance._toolTip.gameObject.SetActive(true);
        Instance._activeTooltip = Instance._toolTip;
    }

    public static void ShowShopTowerTooltip(string header, string body, string cost, string damage, string atkSpeed, string range)
    {
        if (Instance._activeTooltip) Hide();

        Instance._towerToolTip.SetText(header, body, cost, damage, atkSpeed, range);
        Instance._towerToolTip.gameObject.SetActive(true);
        Instance._activeTooltip = Instance._towerToolTip;
    }

    public static void Hide()
    {
        Instance._activeTooltip.gameObject.SetActive(false);
        Instance._activeTooltip = null;
    }
}
