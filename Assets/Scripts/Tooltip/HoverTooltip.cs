using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string header;
    [SerializeField] private string body;
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Show(body, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
    }

    private void OnMouseEnter()
    {
        TooltipManager.Show(body, header);
    }

    private void OnMouseExit()
    {
        TooltipManager.Hide();
    }
}
