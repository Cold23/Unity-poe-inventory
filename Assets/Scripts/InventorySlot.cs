using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Inventory Slot base class
/// </summary>
public class InventorySlot : DropArea
{

    [SerializeField]
    protected GameObject initialItem;
    protected UIItem itemInSlot;
    protected RectTransform rectTransform;
    protected Inventory belongsTo;
    protected Vector2Int position = Vector2Int.zero;

    public virtual void setPos(Vector2Int pos)
    {
        this.position = pos;
    }

    protected virtual void Awake()
    {
        belongsTo = GetComponentInParent<Inventory>();
        rectTransform = GetComponent<RectTransform>();
        if (initialItem != null)
        {
            var obj = GameObject.Instantiate(initialItem, transform);
            itemInSlot = obj.GetComponent<UIItem>();
        }
    }

    public virtual bool CheckCanPlaceItem(UIItem item)
    {
        bool fits = true;
        var count = item.getOccupiesSpots().Count;
        for (var i = 0; i < count; i++)
        {
            var newPos = position + item.getOccupiesSpots()[i];
            var slotData = belongsTo.getSlotItem(newPos);
            var slotItem = slotData?.getItem();
            if (slotData == null || (slotItem != null && slotItem != item))
            {
                fits = false;
                break;
            }
        }
        return fits;
    }

    public override void OnDrop()
    {
        var itemObject = MouseObject.DraggingObject;
        var itemOrigin = itemObject.getOrigin();
        itemOrigin?.onItemRemoved();
        setItem(itemObject);

    }

    public virtual void setItem(UIItem item)
    {
        var count = item.getOccupiesSpots().Count;

        for (var i = 0; i < count; i++)
        {
            belongsTo.setSlotItem(this.position + item.getOccupiesSpots()[i], item);
        }
        itemInSlot.transform.SetParent(transform.parent.parent);
        itemInSlot.transform.SetAsLastSibling();
        var rect = itemInSlot.GetComponent<RectTransform>();
        rect.anchoredPosition = rectTransform.anchoredPosition + new Vector2(-rectTransform.rect.width / 2, rectTransform.rect.height / 2);
        item.setOrigin(this);
    }

    public virtual void setSlotItem(UIItem item)
    {
        itemInSlot = item;
    }

    public virtual void onItemRemoved()
    {
        var lastSlotItem = itemInSlot;
        itemInSlot.setOrigin(null);
        var count = lastSlotItem.getOccupiesSpots().Count;
        for (var i = 0; i < count; i++)
        {
            belongsTo.removeSlotItem(this.position + lastSlotItem.getOccupiesSpots()[i]);
        }
    }

    public UIItem getItem()
    {
        return itemInSlot;
    }
}
