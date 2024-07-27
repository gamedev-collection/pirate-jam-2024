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
    [SerializeField] private TextMeshProUGUI _piercingText;
    [SerializeField] private TextMeshProUGUI _durationText;
    public void SetText(string header, string body, string cost, string damage, string atkSpeed, string range, string piercing = "", string duration = "")
    {
        SetText(body, header);
        if(_costText) _costText.text = cost;
        if(_damageText) _damageText.text = damage;
        if(_attackSpeedText) _attackSpeedText.text = atkSpeed;
        if(_rangeText) _rangeText.text = range;
        if (_piercingText)
        {
            if(piercing != "") _piercingText.text = piercing;
            else _piercingText.transform.parent.gameObject.SetActive(false);
        }
        if (_durationText)
        {
            if (duration != "") _durationText.text = duration;
            else _durationText.transform.parent.gameObject.SetActive(false);
        }
    }
}
