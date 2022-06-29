using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragItem : MonoBehaviour
{
    Image bgImage;
    [SerializeField]
    Image itemImage;
    RectTransform rectTransform;
    bool isBeingDragged = false;
    Controls controls;
    RectTransform canvas;

    private void Awake() {
        bgImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        controls = new Controls();
        canvas = GameObject.FindGameObjectWithTag("UI Canvas").GetComponent<RectTransform>();
    }
    public void setColor(Color color) {
        bgImage.color = color;
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    public void setData(Sprite sprite,Vector2 size){
        rectTransform.sizeDelta = size;
        itemImage.sprite = sprite;
        isBeingDragged = true;
    }


    private void setSelfPosition(Vector2 position)
    {
        position.y -= canvas.rect.height;
        var pos = new Vector2((position.x / rectTransform.lossyScale.x), (position.y / rectTransform.lossyScale.y) + rectTransform.rect.height); // convert to current canvas position

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

    private void Update() {
        if (isBeingDragged)
        {
            var mouse = controls.actions.Mouse.ReadValue<Vector2>() - new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height / 2);
            setSelfPosition(mouse);
        }
    }
}
