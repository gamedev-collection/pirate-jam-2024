using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RuneTooltip : Tooltip
{
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _fieldOne_Title;
    [SerializeField] private TextMeshProUGUI _fieldOne_Value;
    [SerializeField] private TextMeshProUGUI _fieldTwo_Title;
    [SerializeField] private TextMeshProUGUI _fieldTwo_Value;
    [SerializeField] private TextMeshProUGUI _fieldThree_Title;
    [SerializeField] private TextMeshProUGUI _fieldThree_Value;


    public void SetText(string header, string body, string cost, string fieldOne_Title, string fieldOne_Value = "", string fieldTwo_Title = "", string fieldTwo_Value = "", string fieldThree_Title = "", string fieldThree_Value = "")
    {
        SetText(body, header);
        if (_costText) _costText.text = cost;

        if (_fieldOne_Title) _fieldOne_Title.text = fieldOne_Title;
        if (_fieldOne_Value)
        {
            if (fieldOne_Value != "") _fieldOne_Value.text = fieldOne_Value;
            else _fieldOne_Value.transform.parent.gameObject.SetActive(false);
        }

        if (_fieldTwo_Title) _fieldTwo_Title.text = fieldTwo_Title;
        if (_fieldTwo_Value)
        {
            if (fieldTwo_Value != "") _fieldTwo_Value.text = fieldTwo_Value;
            else _fieldTwo_Value.transform.parent.gameObject.SetActive(false);
        }

        if (_fieldThree_Title) _fieldThree_Title.text = fieldThree_Title;
        if (_fieldThree_Value)
        {
            if (fieldThree_Value != "") _fieldThree_Value.text = fieldThree_Value;
            else _fieldThree_Value.transform.parent.gameObject.SetActive(false);
        }

    }
}
