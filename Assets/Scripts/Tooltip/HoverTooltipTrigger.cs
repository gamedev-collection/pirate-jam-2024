using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TooltipType _type;
    [SerializeField] private string _header;
    [SerializeField] private string _body;
    [SerializeField] private GameObject _dataObject;

    public TooltipType Type {  get { return _type; } set { _type = value; } }
    public GameObject DataObject {  get { return _dataObject; } set { _dataObject = value; } }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
    }

    private void OnMouseEnter()
    {
        Show();
    }

    private void OnMouseExit()
    {
        TooltipManager.Hide();
    }

    private void Show()
    {
        switch (_type)
        {
            case TooltipType.Basic: TooltipManager.ShowBasicTooltip(_body, _header); break;
            case TooltipType.ShopTower:
                _dataObject.TryGetComponent<Tower>(out Tower tower);
                TooltipManager.ShowShopTowerTooltip(tower.towerName, tower.towerDescription, tower.cost.ToString(), tower.damage.ToString(), tower.attackRate.ToString(), tower.range.ToString()); break;
        }
    }
}

public enum TooltipType
{
    Basic,
    ShopTower
}
