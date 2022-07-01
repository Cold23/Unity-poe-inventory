using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Customized Inventory slot that represent a socket on a piece of armor/weapon 
/// </summary>
public class Socket : InventorySlot
{
    GemColor acceptsGemColor;
    RectTransform canvas;
    GemItem currentDragObject;
    Image gemColorImage;
    protected override void Awake()
    {
        base.Awake();
        gemColorImage = GetComponent<Image>();
    }

    public void setGemColor(GemColor gemColor)
    {
        this.acceptsGemColor = gemColor;
        switch (gemColor)
        {
            case GemColor.red:
                gemColorImage.color = Color.red;
                break;
            case GemColor.green:
                gemColorImage.color = Color.green;
                break;
            case GemColor.blue:
                gemColorImage.color = Color.blue;
                break;
            case GemColor.white:
                gemColorImage.color = Color.white;
                break;
        }
    }

    public override bool CheckCanPlaceItem(UIItem item)
    {
        var gemObject = item.gameObject.GetComponent<GemItem>();
        if (gemObject == null || (acceptsGemColor != GemColor.white && acceptsGemColor != gemObject.gemColor)) return false;
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
