using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;


    Controls controls;
    RectTransform rectTransform;
    [SerializeField]
    RectTransform canvas;
    [SerializeField]
    TMP_Text text;
    [SerializeField]
    TMP_Text title;
    [SerializeField]
    GameObject innerPanel;
    [SerializeField]
    GameObject divider;
    Image innerPanelImage;
    Image outerPanelImage;
    Image dividerImage;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        resizeTooltip();
        controls = new Controls();
        hideTooltip(true);

    }

    private void resizeTooltip()
    {
        var dividerRect = divider.GetComponent<RectTransform>();
        var innerRect = innerPanel.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        innerPanelImage = innerPanel.GetComponent<Image>();
        outerPanelImage = gameObject.GetComponent<Image>();
        dividerImage = divider.GetComponent<Image>();

        dividerRect.anchoredPosition = new Vector2(0, -(title.preferredHeight + 10));
        innerRect.sizeDelta = new Vector2(395, text.preferredHeight + title.preferredHeight + 20);
        rectTransform.sizeDelta = innerRect.sizeDelta + Vector2.one * 5;
    }

    public void hideTooltip(bool instant = false)
    {
        DOTween.Kill("fade");
        if (instant)
        {
            title.DOFade(0, 0.2f).SetId("fade");
            text.DOFade(0, 0.2f).SetId("fade");
            innerPanelImage.DOFade(0, 0.2f).SetId("fade");
            outerPanelImage.DOFade(0, 0.2f).SetId("fade");
            dividerImage.DOFade(0, 0.2f).SetId("fade");
            return;
        }
    }

    public void showTooltip(string titleText, string content, Vector2 position, Vector2 sizeDelta)
    {
        DOTween.Kill("fade");
        title.text = titleText;
        text.text = content;
        resizeTooltip();
        setTooltipPosition(position + new Vector2(-rectTransform.rect.width / 2 + sizeDelta.x / 2, sizeDelta.y + 20f));

        title.DOFade(1f, 0.2f).SetId("fade");
        text.DOFade(1f, 0.2f).SetId("fade");
        innerPanelImage.DOFade(1f, 0.2f).SetId("fade");
        outerPanelImage.DOFade(1f, 0.2f).SetId("fade");
        dividerImage.DOFade(1f, 0.2f).SetId("fade");

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void setTooltipPosition(Vector2 position)
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

    private void Update()
    {
        // var mousePos = controls.actions.Mouse.ReadValue<Vector2>();
        // setTooltipPosition(mousePos);
    }

}
