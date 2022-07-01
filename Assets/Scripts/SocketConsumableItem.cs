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
    [SerializeField]
    int ItemCount = 25;
    [SerializeField]
    TMP_Text text;

    public override void Awake()
    {
        base.Awake();
        setItemCount(ItemCount);

    }

    void setItemCount(int count)
    {
        ItemCount = count;
        text.text = ItemCount > 1 ? ItemCount.ToString() : "";

    }
    public override void onMouseOver()
    {
        var text = "Reforged the amount of sockets on an item";

        TooltipManager.instance.showTooltip("Jewelers Orb ", text, rectTransform.position, rectTransform.sizeDelta, gameObject.GetInstanceID());
    }

    public virtual void onItemUsed()
    {
        setItemCount(ItemCount - 1);

    }

    public override void onMouseMoved()
    {
        if (activeDragObject == null) return;

        PointerEventData evData = new PointerEventData(EventSystem.current);

        evData.position = controls.actions.Mouse.ReadValue<Vector2>() + new Vector2(-rectTransform.rect.width / 2, rectTransform.rect.height / 2);


        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(evData, results);
        if (results.Count <= 0)
        {
            hoveredSlot = null;
            canPlaceItem = false;
            activeDragObject.setColor(baseColor);
            return;
        }
        List<InventorySlot> slots = results.Select((el, index) =>
        {
            var script = el.gameObject.GetComponent<InventorySlot>();
            if (script != null)
                return script;
            else
            {
                return null;
            }
        }).Where((el) => el != null).ToList<InventorySlot>();
        if (slots.Count <= 0)
        {
            activeDragObject.setColor(baseColor);
            return;
        }
        checkSlots(slots);
    }
}
