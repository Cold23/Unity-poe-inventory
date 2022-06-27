using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InventorySlot : DropArea
{

    [SerializeField]
    GameObject initialItem;
    UIItem itemInSlot;
    RectTransform rectTransform;
    Inventory belongsTo;
    TMP_Text text;
    Vector2Int position = Vector2Int.zero;

    public virtual void setPos(Vector2Int pos) {
        this.position = pos;
        text.text = pos.x.ToString() + "," + pos.y.ToString();
    }

    protected virtual void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        belongsTo = GetComponentInParent<Inventory>();
        rectTransform = GetComponent<RectTransform>();
        if (initialItem != null)
        {
            var obj = GameObject.Instantiate(initialItem, transform);
            itemInSlot = obj.GetComponent<UIItem>();
        }
    }

    public virtual bool CheckItemFits(UIItem item) {
        bool fits = true;
        for (var i = 0; i < item.occupiesSpots.Count; i ++) {
            var newPos = position + item.occupiesSpots[i];
            var isEmpty = belongsTo.isSlotEmptyAtPos(newPos);
            if(!isEmpty) {
                fits = false;
                break;
            }
        }
        return fits;
    }

    public override void OnDrop()
    {
        var itemObject = MouseObject.DraggingObject;
        var itemScript = itemObject.GetComponent<UIItem>();
        var itemOrigin = itemScript.getOrigin();
        if (itemOrigin != null && itemInSlot != null)
        {
            var temp = itemInSlot;
            setItem(itemObject);
            itemOrigin.setItem(temp);
        }
        else
        {
            setItem(itemObject);
        }

    }

    public virtual void setItem(UIItem item)
    {
        for (var i = 0; i < item.occupiesSpots.Count; i++)
        {
            belongsTo.setSlotItem(this.position + item.occupiesSpots[i], item);
        }
        itemInSlot.transform.SetParent(transform.parent.parent);
        itemInSlot.transform.SetAsLastSibling();
        var rect = itemInSlot.GetComponent<RectTransform>();
        rect.anchoredPosition = rectTransform.anchoredPosition - new Vector2(+30,rect.rect.height-30);
        item.setOrigin(this);
    }

    public virtual void setSlotItem(UIItem item) {
        itemInSlot = item;
    }

    public virtual void onItemRemoved()
    {
        var lastSlotItem = itemInSlot;
        itemInSlot.setOrigin(null);
        for (var i = 0; i < lastSlotItem.occupiesSpots.Count; i++)
        {
            belongsTo.removeSlotItem(this.position + lastSlotItem.occupiesSpots[i]);
        }
    }

    public UIItem getItem()
    {
        return itemInSlot;
    }
}
