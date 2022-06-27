using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : TooltipHoverObject, IDraggableItem
{
    protected Controls controls;
    public List<Vector2Int> occupiesSpots;
    protected RectTransform rectTransform;
    protected bool isBeingDragged = false;
    protected RectTransform canvas;
    protected RectTransform dragCanvas;
    Color baseColor = new Color(0.6f, 0.6f, 0.6f, 0.4f);
    protected Image bgImage;
    protected InventorySlot originatedFrom;
    protected bool fitsInventory = false;
    protected InventorySlot hoveredSlot;

    public void setOrigin(InventorySlot from)
    {
        originatedFrom = from;
    }

    public InventorySlot getOrigin()
    {
        return originatedFrom;
    }
    public virtual void DisableRaycast()
    {
        gameObject.GetComponent<Image>().raycastTarget = false;
    }

    public virtual void EnableRaycast()
    {
        gameObject.GetComponent<Image>().raycastTarget = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originatedFrom?.onItemRemoved();
        DisableRaycast();
        MouseObject.set(this);
        isBeingDragged = true;
        transform.SetParent(dragCanvas);
        transform.SetAsLastSibling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EnableRaycast();
        MouseObject.clear();
        isBeingDragged = false;

    }

    public InventorySlot getHovered() {
        return hoveredSlot;
    }

    public override void onMouseMoved()
    {
        if(!isBeingDragged) return;

        PointerEventData evData = new PointerEventData(EventSystem.current);

        evData.position = controls.actions.Mouse.ReadValue<Vector2>() + new Vector2(-rectTransform.rect.width/2,rectTransform.rect.height/2);


        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(evData, results);

        results.RemoveAll((el) =>
        {
            var slotObject = el.gameObject.GetComponent<InventorySlot>();
            if(slotObject == null) {
                return true;
            }
            fitsInventory = slotObject.CheckItemFits(this);
            if(fitsInventory){
                hoveredSlot = slotObject;
                bgImage.color = new Color(0,1,0,0.4f);
            }else {
                hoveredSlot = null;
                bgImage.color = new Color(1,0,0,0.4f);
            }
            return false;
        });
        if(results.Count <= 0) {
            hoveredSlot = null;
            fitsInventory = false;
            bgImage.color = baseColor;
        }
        
    }
}

public class GemItem : UIItem
{

    [SerializeField] Image dragImage;

    private void Awake()
    {
        bgImage = GetComponent<Image>();
        dragCanvas = GameObject.FindGameObjectWithTag("Drag Canvas").GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("UI Canvas").GetComponent<RectTransform>();
        controls = new Controls();
        rectTransform = GetComponent<RectTransform>();
    }

    public override void onShowTooltip()
    {
        var chars = "ABCDEFGH IJKLMNO PQRSTUVW XYZa bcdef ghijklm nopqrstuvwxyz0123456789     ";
        var stringChars = new char[Random.Range(80, 400)];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[Random.Range(0, chars.Length)];
        }

        var finalString = new string(stringChars);
        TooltipManager.instance.showTooltip("Rain gem", finalString, rectTransform.position, rectTransform.sizeDelta, gameObject.GetInstanceID());
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

    private void Update()
    {
        if (isBeingDragged)
        {
            var mouse = controls.actions.Mouse.ReadValue<Vector2>() - new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height / 2);
            setSelfPosition(mouse);
        }
    }

    private void setSelfPosition(Vector2 position)
    {
        position.y -= canvas.rect.height;
        var pos = new Vector2((position.x / rectTransform.lossyScale.x), (position.y / rectTransform.lossyScale.y)); // convert to current canvas position

        if (pos.x < 0)
        {
            pos.x = 0;
        }
        else if (pos.x > canvas.rect.width - rectTransform.rect.width)
        {
            pos.x = canvas.rect.width - rectTransform.rect.width;
        }

        if (pos.y > -rectTransform.rect.height)
        {
            pos.y = -rectTransform.rect.height;
        }
        else if (pos.y < -canvas.rect.height)
        {
            pos.y = -canvas.rect.height;
        }

        rectTransform.anchoredPosition = pos;
    }

}


public interface IDraggableItem : IPointerDownHandler, IPointerUpHandler
{
}
