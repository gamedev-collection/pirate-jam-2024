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

    public string Header { set { _header = value; } }
    public string Body { set { _body = value; } }
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

    public void Show()
    {
        switch (_type)
        {
            case TooltipType.Basic: TooltipManager.ShowBasicTooltip(_body, _header); break;
            case TooltipType.ShopTower:
                _dataObject.TryGetComponent<Tower>(out Tower tower);
                TooltipManager.ShowShopTowerTooltip(tower.towerName, tower.towerDescription, tower.cost.ToString(), tower.damage.ToString(), tower.attackRate.ToString(), tower.range.ToString()); break;
            case TooltipType.ShopBuffRune:
                _dataObject.TryGetComponent<BuffRune>(out BuffRune buffRune);
                TooltipManager.ShowShopTowerTooltip(buffRune.runeName, buffRune.runeDescription, buffRune.cost.ToString(), buffRune.damageBuff.ToString(), buffRune.attackRateBuff.ToString(), buffRune.rangeBuff.ToString()); break;
            case TooltipType.ShopFireRune:
                _dataObject.TryGetComponent<FireRune>(out FireRune fireRune);
                TooltipManager.ShowShopRuneTooltip(fireRune.runeName, fireRune.runeDescription, fireRune.cost.ToString(), "Damage per tick", fireRune.damagePerTick.ToString(), "Ticks", fireRune.tickTimes.ToString()); break;
            case TooltipType.ShopFreezeRune:
                _dataObject.TryGetComponent<FreezeRune>(out FreezeRune freezeRune);
                TooltipManager.ShowShopRuneTooltip(freezeRune.runeName, freezeRune.runeDescription, freezeRune.cost.ToString(), "Slow amount", freezeRune.slowAmount.ToString(), "Duration", freezeRune.duration.ToString()); break;
        }
    }
}

public enum TooltipType
{
    Basic,
    ShopTower,
    ShopBuffRune,
    ShopFireRune,
    ShopFreezeRune
}
