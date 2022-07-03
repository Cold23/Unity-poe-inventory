using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The transparent object that appear after dragging an object
/// </summary>
public class DragItem : MonoBehaviour
{
    Image bgImage;
    [SerializeField]
    Image itemImage;
    RectTransform rectTransform;
    bool isBeingDragged = false;
    Controls controls;
    RectTransform canvas;

    private void Awake()
    {
        bgImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        controls = new Controls();
        canvas = GameObject.FindGameObjectWithTag("UI Canvas").GetComponent<RectTransform>();
    }
    public void setColor(Color color)
    {
        bgImage.color = color;
    }

    private void OnEnable()
    {
        setPositionToMouse();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void setData(Sprite sprite, Vector2 size)
    {
        rectTransform.sizeDelta = size;
        itemImage.sprite = sprite;
        isBeingDragged = true;
    }


    private void setSelfPosition(Vector2 position)
    {
        var pos = new Vector2((position.x / canvas.lossyScale.x), (position.y / canvas.lossyScale.y)); // convert to current canvas position
        // limit x position to fit on screen
        pos.y -= canvas.rect.height;
        if (pos.x < 0)
        {
            pos.x = 0;
        }
        else if (pos.x > canvas.rect.width - rectTransform.rect.width)
        {
            pos.x = canvas.rect.width - rectTransform.rect.width;
        }
        // limit yPos to fit on screen

        if (pos.y > 0)
        {
            pos.y = 0;
        }
        else if (pos.y < -canvas.rect.height + rectTransform.rect.height)
        {
            pos.y = -canvas.rect.height + rectTransform.rect.height;
        }

        // set position
        rectTransform.anchoredPosition = pos;
    }

    void setPositionToMouse()
    {
        var mouse = controls.actions.Mouse.ReadValue<Vector2>() - new Vector2(rectTransform.rect.width * rectTransform.lossyScale.x / 2, -rectTransform.rect.height * rectTransform.lossyScale.y / 2);
        setSelfPosition(mouse);
    }

    private void Update()
    {
        if (isBeingDragged)
        {
            setPositionToMouse();
        }
    }
}
