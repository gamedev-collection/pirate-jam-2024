using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerTooltip : Tooltip
{
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private TextMeshProUGUI _attackSpeedText;
    [SerializeField] private TextMeshProUGUI _rangeText;
    public void SetText(string header, string body, string cost, string damage, string atkSpeed, string range)
    {
        SetText(body, header);
        if(_costText) _costText.text = cost;
        if(_damageText) _damageText.text = damage;
        if(_attackSpeedText) _attackSpeedText.text = atkSpeed;
        if(_rangeText) _rangeText.text = range;
    }
}
