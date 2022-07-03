using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Items that are used to either reforge socket count, socket color or 
/// socket links.
/// These items can be drag and dropped on top of items that have socked
/// triggering their respective onItemUsed function
/// </summary>
public class SocketConsumableItem : UIItem
{
    int ItemCount;
    [SerializeField]
    TMP_Text text;
    UIItem currentActiveItem;

    public override void Awake()
    {
        base.Awake();
        setItemCount(itemData.startAmount);

    }

    void setItemCount(int count)
    {
        ItemCount = count;
        text.text = ItemCount > 1 ? ItemCount.ToString() : "";

    }

    public virtual void onItemUsed()
    {
        currentActiveItem.getSockets().RerollSocketsAndConnections();
        currentActiveItem = null;
        setItemCount(ItemCount - 1);
        if (ItemCount <= 0)
        {
            Destroy(this.gameObject);
        }

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        EnableRaycast();
        Destroy(activeDragObject.gameObject);
        activeDragObject = null;

        // if over socket item use on socketedItem
        // else place item ( if possible )
        if (currentActiveItem != null)
        {
            MouseObject.clearActiveItem();
            onItemUsed();
            return;
        }
        MouseObject.placeActiveItem();
    }

    public override void onMouseMoved()
    {
        base.onMouseMoved();

        if (activeDragObject == null) return;

        PointerEventData evData = new PointerEventData(EventSystem.current);

        ///check the mouse position aka center of the dragging item
        evData.position = controls.actions.Mouse.ReadValue<Vector2>();


        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(evData, results);
        if (results.Count <= 0)
        {
            currentActiveItem = null;
            return;
        }
        List<UIItem> items = results.Select((el, index) =>
        {
            var script = el.gameObject.GetComponent<UIItem>();
            if (script != null)
                return script;
            else
            {
                return null;
            }
        }).Where((el) => el != null && el.hasSockets() && el.getSockets().areSocketsEmpty()).ToList<UIItem>();
        if (items.Count <= 0)
        {
            currentActiveItem = null;
            return;
        }
        activeDragObject.setColor(baseColor);
        currentActiveItem = items[0];
    }

}
