using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TooltipManager : Singleton<TooltipManager>
{
    [SerializeField] private Tooltip _toolTip;
    [SerializeField] private TowerTooltip _towerToolTip;
    [SerializeField] private RuneTooltip _runeToolTip;

    public Tooltip Tooltip { get { return _toolTip; } }
    public TowerTooltip TowerTooltip { get { return _towerToolTip; } }
    public RuneTooltip RuneTooltip { get { return _runeToolTip; } }

    public Tooltip ActiveTooltip { get; private set; }

    public static void ShowBasicTooltip(string body, string header = "")
    {
        if (Instance.ActiveTooltip) Hide();

        Instance._toolTip.SetText(body, header);
        Instance._toolTip.gameObject.SetActive(true);
        Instance.ActiveTooltip = Instance._toolTip;
    }

    public static void ShowShopTowerTooltip(string header, string body, string cost, string damage, string atkSpeed, string range)
    {
        if (Instance.ActiveTooltip) Hide();

        Instance._towerToolTip.SetText(header, body, cost, damage, atkSpeed, range);
        Instance._towerToolTip.gameObject.SetActive(true);
        Instance.ActiveTooltip = Instance._towerToolTip;
    }

    public static void ShowShopRuneTooltip(string header, string body, string cost, string fieldOne_Title = "", string fieldOne_Value = "", string fieldTwo_Title = "", string fieldTwo_Value = "", string fieldThree_Title = "", string fieldThree_Value = "")
    {
        if (Instance.ActiveTooltip) Hide();

        Instance._runeToolTip.SetText(header, body, cost, fieldOne_Title, fieldOne_Value, fieldTwo_Title, fieldTwo_Value, fieldThree_Title, fieldThree_Value);
        Instance._runeToolTip.gameObject.SetActive(true);
        Instance.ActiveTooltip = Instance._runeToolTip;
    }

    public static void Hide()
    {
        Instance.ActiveTooltip.gameObject.SetActive(false);
        Instance.ActiveTooltip = null;
    }
}
