using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : ItemHover, IDraggableItem
{
    protected Controls controls;
    public List<Vector2Int> occupiesSpots;
    protected RectTransform rectTransform;
    protected RectTransform canvas;
    protected RectTransform dragCanvas;
    Color baseColor = new Color(0.6f, 0.6f, 0.6f, 0.4f);
    protected Image bgImage;
    protected InventorySlot originatedFrom;
    protected bool fitsInventory = false;
    protected InventorySlot hoveredSlot;
    [SerializeField]

    protected GameObject dragObject;
    DragItem activeDragObject;
    [SerializeField]
    protected Sprite itemSprite;

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
        gameObject.GetComponent<Image>().raycastTarget = false;
    }

    public virtual void EnableRaycast()
    {
        gameObject.GetComponent<Image>().raycastTarget = true;
    }

    public override void onMouseOver( ) {
        var color = baseColor;
        color.a = 0.8f;
        bgImage.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var gameObject = GameObject.Instantiate(dragObject, dragCanvas);
        activeDragObject = gameObject.GetComponent<DragItem>();
        activeDragObject.setData(itemSprite, rectTransform.sizeDelta);
        DisableRaycast();
        MouseObject.set(this);

        activeDragObject.transform.SetParent(dragCanvas);
        activeDragObject.transform.SetAsLastSibling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EnableRaycast();
        MouseObject.clear();
        Destroy(activeDragObject.gameObject);
        activeDragObject = null;
    }

    public InventorySlot getHovered() {
        return hoveredSlot;
    }

    public override void onMouseMoved()
    {
        if(activeDragObject == null) return;

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
                activeDragObject.setColor(new Color(0, 1, 0, 0.4f));
            }else {
                hoveredSlot = null;
                activeDragObject.setColor(new Color(1, 0, 0, 0.4f));
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
    private void Awake()
    {
        bgImage = GetComponent<Image>();
        dragCanvas = GameObject.FindGameObjectWithTag("Drag Canvas").GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("UI Canvas").GetComponent<RectTransform>();
        controls = new Controls();
        rectTransform = GetComponent<RectTransform>();
    }

    public override void onMouseOver()
    {
        base.onMouseOver();
        var chars = "ABCDEFGH IJKLMNO PQRSTUVW XYZa bcdef ghijklm nopqrstuvwxyz0123456789     ";
        var stringChars = new char[Random.Range(80, 400)];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[Random.Range(0, chars.Length)];
        }

        var finalString = new string(stringChars);
        TooltipManager.instance.showTooltip("Rain gem", finalString, rectTransform.position, rectTransform.sizeDelta, gameObject.GetInstanceID());
    }

 



    

}


public interface IDraggableItem : IPointerDownHandler, IPointerUpHandler
{
}
