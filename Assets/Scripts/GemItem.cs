using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum GemColor
{
    red,
    green,
    blue,
    white
}
/// <summary>
/// Base class that all items that have a ui representation should inherit from
/// </summary>
public class UIItem : ItemHover, IDraggableItem
{
    [SerializeField]
    protected ItemBase itemData;
    protected SocketLayoutController sockets;
    protected Controls controls;
    protected RectTransform rectTransform;
    protected RectTransform canvas;
    protected RectTransform dragCanvas;
    protected Color baseColor = new Color(0.6f, 0.6f, 0.6f, 0.4f);
    protected Image bgImage;
    [SerializeField]
    protected Image itemImage;

    protected InventorySlot originatedFrom;
    protected bool canPlaceItem = false;
    protected InventorySlot hoveredSlot;
    [SerializeField]

    protected GameObject dragObject;
    protected DragItem activeDragObject;
    [SerializeField]
    public virtual void Awake()
    {
        bgImage = GetComponent<Image>();
        dragCanvas = GameObject.FindGameObjectWithTag("Drag Canvas").GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("UI Canvas").GetComponent<RectTransform>();
        controls = new Controls();
        rectTransform = GetComponent<RectTransform>();
        itemImage.sprite = itemData.itemSprite;
    }

    public List<Vector2Int> getOccupiesSpots()
    {
        return itemData.occupiesSpots;
    }

    public SocketLayoutController getSockets()
    {
        return sockets;
    }

    public virtual bool hasSockets()
    {
        return this.sockets != null;
    }

    public virtual void bindSockets(SocketLayoutController sockets)
    {
        this.sockets = sockets;
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.actions.MouseMove.performed += _ =>
        {
            onMouseMoved();
        };
    }

    private void OnDisable()
    {
        controls.Disable();

    }

    public void setOrigin(InventorySlot from)
    {
        originatedFrom = from;
        bgImage.color = baseColor;

    }

    public override void onMouseOut()
    {

        bgImage.color = baseColor;
    }

    public InventorySlot getOrigin()
    {
        return originatedFrom;
    }
    public virtual void DisableRaycast()
    {
        bgImage.raycastTarget = false;
    }

    public virtual void EnableRaycast()
    {
        bgImage.raycastTarget = true;
    }

    public override void onMouseOver()
    {
        var color = baseColor;
        color.a = 0.8f;
        bgImage.color = color;

        TooltipManager.instance.showTooltip(itemData.itemTitle, itemData.itemDescription, rectTransform.position, rectTransform.sizeDelta,
        gameObject.GetInstanceID());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var gameObject = GameObject.Instantiate(dragObject, dragCanvas);
        activeDragObject = gameObject.GetComponent<DragItem>();
        activeDragObject.setData(itemData.itemSprite, rectTransform.sizeDelta);
        DisableRaycast();
        MouseObject.set(this);

        activeDragObject.transform.SetParent(dragCanvas);
        activeDragObject.transform.SetAsLastSibling();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        EnableRaycast();
        MouseObject.placeActiveItem();
        Destroy(activeDragObject.gameObject);
        activeDragObject = null;
    }

    public InventorySlot getHovered()
    {
        return hoveredSlot;
    }

    public virtual void checkCanBePlaced(List<InventorySlot> slots)
    {
        foreach (var slot in slots)
        {
            canPlaceItem = slot.CheckCanPlaceItem(this);
            if (canPlaceItem)
            {
                hoveredSlot = slot;
                activeDragObject.setColor(new Color(0, 1, 0, 0.4f));
                return;
            }
            else
            {
                hoveredSlot = null;
                activeDragObject.setColor(new Color(1, 0, 0, 0.4f));
            }
        }
    }

    public override void onMouseMoved()
    {
        if (activeDragObject == null) return;

        PointerEventData evData = new PointerEventData(EventSystem.current);

        evData.position = controls.actions.Mouse.ReadValue<Vector2>() + new Vector2(-rectTransform.rect.width * rectTransform.lossyScale.x / 2, rectTransform.rect.height * rectTransform.lossyScale.y / 2);


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
            hoveredSlot = null;
            return;
        }
        checkCanBePlaced(slots);
    }
}

/// <summary>
/// A gem type item that can be socketed into sockets ( duh ..)
/// </summary>
public class GemItem : UIItem
{
    public GemColor gemColor;

}

/// <summary>
/// Interface for implementing dragging of items ( maybe other objects too )
/// </summary>
public interface IDraggableItem : IPointerDownHandler, IPointerUpHandler
{
}
