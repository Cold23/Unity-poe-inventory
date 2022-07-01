using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class Containing values for the current object being dragged etc..
/// </summary>
public static class MouseObject
{
    static bool dragging = false;
    static DropArea hoveredDropArea;
    static UIItem draggingItem;
    static Inventory mouseOverInventory;

    public static bool Dragging { get => dragging; set => dragging = value; }
    public static UIItem DraggingObject { get => draggingItem; set => draggingItem = value; }

    public static void setHovered(DropArea area)
    {
        hoveredDropArea = area;
    }
    public static void clearHovered()
    {
        hoveredDropArea = null;
    }

    public static void setInventory(Inventory inv)
    {
        mouseOverInventory = inv;
    }
    public static void clearInventory()
    {
        mouseOverInventory = null;
    }

    public static void set(UIItem obj)
    {
        dragging = true;
        draggingItem = obj;
    }

    public static void clear()
    {
        var slotToBePlacedAt = draggingItem.getHovered();
        if (slotToBePlacedAt != null)
        {
            slotToBePlacedAt.OnDrop();
            // hoveredDropArea.OnDrop();
        }
        dragging = false;
        draggingItem = null;
    }
}
