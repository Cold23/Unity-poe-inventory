using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TooltipHoverObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        onShowTooltip();

    }

    public virtual void onShowTooltip()
    {

    }

    public virtual void onMouseMoved(){}

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.instance.hideTooltip(true);

    }

    public void OnPointerMove(PointerEventData eventData)
    {
    }
}
