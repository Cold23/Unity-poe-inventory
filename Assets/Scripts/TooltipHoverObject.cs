using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Items that have tooltips ( all of them in my case ) should inherit from this class
/// to have a tooltip shown
/// </summary>
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

    public virtual void onMouseMoved() { }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseOut();
        TooltipManager.instance.hideTooltip(true);

    }
    public void OnPointerMove(PointerEventData eventData)
    {
    }
}
