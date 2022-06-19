using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseObject
{
    static bool dragging = false;
    static DropArea hoveredDropArea;
    static GameObject draggingObject;

    public static bool Dragging { get => dragging; set => dragging = value; }
    public static GameObject DraggingObject { get => draggingObject; set => draggingObject = value; }

    public static void setHovered(DropArea area)
    {
        hoveredDropArea = area;
    }
    public static void clearHovered()
    {
        hoveredDropArea = null;
    }

    public static void set(GameObject obj)
    {
        dragging = true;
        draggingObject = obj;
    }

    public static void clear()
    {
        if (hoveredDropArea != null)
        {
            hoveredDropArea.OnDrop();
        }
        dragging = false;
        draggingObject = null;
    }
}
