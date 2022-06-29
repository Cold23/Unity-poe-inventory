using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GenericItem : UIItem
{
    [SerializeField]
    Image mainItemImage;


    private void Awake()
    {
        bgImage = GetComponent<Image>();
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

    public override void onMouseOver()
    {
        base.onMouseOver();
        var chars = "ABCDEFGH IJKLMNO PQRSTUVW XYZa bcdef ghijklm nopqrstuvwxyz0123456789     ";
        var stringChars = new char[80];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[Random.Range(0, chars.Length)];
        }

        var finalString = new string(stringChars);
        TooltipManager.instance.showTooltip("Supa armor", finalString, rectTransform.position, rectTransform.sizeDelta, gameObject.GetInstanceID());
    }
    
}
