using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : DropArea
{
    [SerializeField]
    GameObject initialItem;
    GameObject itemInSlot;
    RectTransform rectTransform;
    Inventory belongsTo;

    protected virtual void Awake()
    {
        belongsTo = GetComponentInParent<Inventory>();
        Debug.Log(belongsTo);
        rectTransform = GetComponent<RectTransform>();
        if (initialItem != null)
        {
            itemInSlot = GameObject.Instantiate(initialItem, transform);
        }
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

    public virtual void setItem(GameObject item)
    {
        itemInSlot = item;
        itemInSlot.transform.SetParent(transform);
        var rect = itemInSlot.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rectTransform.rect.width / 2 - rect.rect.width / 2, rectTransform.rect.height / 2 - rect.rect.height / 2);

        var itemScript = item.GetComponent<UIItem>();
        itemScript.setOrigin(this);

    }

    public void removeItem()
    {
        itemInSlot = null;
    }

    public GameObject getItem()
    {
        return itemInSlot;
    }
}
