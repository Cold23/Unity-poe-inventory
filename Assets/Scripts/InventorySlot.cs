using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InventorySlot : DropArea
{
    [SerializeField]
    RectTransform itemParent;
    [SerializeField]
    GameObject initialItem;
    GameObject itemInSlot;
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
            itemInSlot = GameObject.Instantiate(initialItem, transform);
        }
    }

    public virtual bool CheckItemFits(UIItem item) {
        bool fits = true;
        Debug.Log("currently on pos: " +position.ToString());
        for (var i = 0; i < item.occupiesSpots.Count; i ++) {
            var newPos = position + item.occupiesSpots[i];
            var isEmpty = belongsTo.isSlotEmptyAtPos(newPos);
            if(!isEmpty) {
                Debug.Log(newPos);
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

    public virtual void setItem(GameObject item)
    {
        itemInSlot = item;
        itemInSlot.transform.SetParent(itemParent);
        var rect = itemInSlot.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(itemParent.rect.width / 2 - rect.rect.width / 2, -(itemParent.rect.height / 2 - rect.rect.height / 2 +rect.rect.height));

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
