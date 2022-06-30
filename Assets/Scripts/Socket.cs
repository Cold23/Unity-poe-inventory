using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Socket : InventorySlot
{
    RectTransform canvas;
    GemItem currentDragObject;
    protected override void Awake()
    {
        base.Awake();
    }

    public override bool CheckCanPlaceItem(UIItem item)
    {
        var gemObject = item.gameObject.GetComponent<GemItem>();
        if (gemObject == null) return false;
        return itemInSlot == null;
    }

    public override void setItem(UIItem item)
    {
        setSlotItem(item);
        itemInSlot.transform.SetParent(transform.GetChild(0));
        var rect = itemInSlot.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(-rect.rect.width / 2 + rectTransform.rect.width / 2, rect.rect.height / 2 - rectTransform.rect.height / 2);
        item.setOrigin(this);
    }
}


public class DropArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (MouseObject.Dragging)
            MouseObject.setHovered(this);
    }

    public virtual void OnDrop() { }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        MouseObject.clearHovered();

    }
}
