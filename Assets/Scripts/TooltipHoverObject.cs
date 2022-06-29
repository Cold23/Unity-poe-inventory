using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseOver();

    }

    public virtual void onMouseOver()
    {

    }

    public virtual void onMouseOut()
    {

    }

    public virtual void onMouseMoved(){}

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseOut();
        TooltipManager.instance.hideTooltip(true);

    }
    public void OnPointerMove(PointerEventData eventData)
    {
    }
}
