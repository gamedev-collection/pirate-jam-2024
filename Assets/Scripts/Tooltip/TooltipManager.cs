using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TooltipManager : Singleton<TooltipManager>
{
    [SerializeField] private Tooltip _toolTip;


    private void Awake()
    {
        Hide();
    }
    public static void Show(string body, string header ="")
    {
        Instance._toolTip.SetText(body, header);
        Instance._toolTip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        Instance._toolTip.gameObject.SetActive(false);
    }
}
