using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TooltipHoverObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        onShowTooltip();
    }

    public virtual void onShowTooltip()
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.instance.hideTooltip(true);

    }
}
