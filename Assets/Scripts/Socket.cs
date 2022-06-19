using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Socket : InventorySlot
{
    [SerializeField]
    GameObject gemItem;

    RectTransform canvas;
    GemItem currentDragObject;
    protected override void Awake()
    {
        base.Awake();
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
