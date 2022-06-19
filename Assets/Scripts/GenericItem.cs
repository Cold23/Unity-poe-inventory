using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GenericItem : UIItem
{
    RectTransform rectTransform;

    Controls controls;
    [SerializeField]
    Image mainItemImage;



    private void Awake()
    {
        dragCanvas = GameObject.FindGameObjectWithTag("Drag Canvas").GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("UI Canvas").GetComponent<RectTransform>();
        controls = new Controls();
        rectTransform = GetComponent<RectTransform>();
    }

    public override void DisableRaycast()
    {
        mainItemImage.raycastTarget = false;
    }

    public override void EnableRaycast()
    {
        mainItemImage.raycastTarget = true;

    }

    public override void onShowTooltip()
    {
        var chars = "ABCDEFGH IJKLMNO PQRSTUVW XYZa bcdef ghijklm nopqrstuvwxyz0123456789     ";
        var stringChars = new char[80];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[Random.Range(0, chars.Length)];
        }

        var finalString = new string(stringChars);
        TooltipManager.instance.showTooltip("Supa armor", finalString, rectTransform.position, rectTransform.sizeDelta, gameObject.GetInstanceID());
    }

    private void OnEnable()
    {
        controls.Enable();
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

        var pos = new Vector2((position.x / rectTransform.lossyScale.x), (position.y / rectTransform.lossyScale.y)); // convert to current canvas position

        if (pos.x < 0)
        {
            pos.x = 0;
        }
        else if (pos.x > canvas.rect.width - rectTransform.rect.width)
        {
            pos.x = canvas.rect.width - rectTransform.rect.width;
        }

        if (pos.y < 0)
        {
            pos.y = 0;
        }
        else if (pos.y + rectTransform.rect.height > canvas.rect.height)
        {
            pos.y = canvas.rect.height - rectTransform.rect.height;
        }

        rectTransform.anchoredPosition = pos;
    }
}
