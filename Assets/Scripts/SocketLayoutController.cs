using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory for the sockets.
/// Handle things such as what gems are in the socket, reforging socket properties etc.
/// </summary>
public class SocketLayoutController : Inventory
{
    [SerializeField]
    UIItem belongsToItem;
    [SerializeField]
    Transform connectionsParent;
    [SerializeField]
    GameObject connectionLinePrefab;
    [SerializeField]
    int maxSocketCount = 6;

    #region socket Reroll test

    Controls controls;

    private void OnEnable()
    {
        controls = new Controls();
        controls.Enable();
        controls.actions.Reroll.performed += _ =>
        {
            RerollSocketsAndConnections();
        };
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    #endregion

    List<Vector2Int> positions = new List<Vector2Int>()
    {
        new Vector2Int(0,0),
        new Vector2Int(1,0),
        new Vector2Int(1,1),
        new Vector2Int(0,1),
        new Vector2Int(0,2),
        new Vector2Int(1,2),
    };
    RectTransform rectTransform;
    float spacingX;
    float spacingY;

    List<RectTransform> socketTransforms = new List<RectTransform>();

    // ovveride inventory init func for sockets
    protected override void init()
    {
        slotData = new Dictionary<Vector2Int, InventorySlotData>();

        rectTransform.sizeDelta = new Vector2(150, 150);
        setSize(UnityEngine.Random.Range(1, maxSocketCount + 1));

        var rows = Mathf.CeilToInt(getSize() / 2f);
        spacingY = (rectTransform.sizeDelta.y - (rows * itemSize)) / (rows - 1);
        spacingX = Mathf.Clamp((rectTransform.sizeDelta.x - 2 * itemSize), 0, itemSize);
        spacingY = Mathf.Clamp(spacingY, 0, itemSize);

        for (int i = 0; i < getSize(); i++)
        {
            var socketObject = GameObject.Instantiate(emptySlotPrefab, transform);
            var socketScript = socketObject.GetComponent<Socket>();
            RectTransform socketRect = (RectTransform)socketObject.transform;
            socketTransforms.Add(socketRect);
            LayoutSocket(socketRect, socketScript, i);
            if (i > 0)
            {
                connectSockets(socketRect, socketTransforms[i - 1], i);
            }

        }

        var rectSize = new Vector2(itemSize * 2 + spacingX, itemSize * rows + spacingY * (rows - 1));
        makeConnections();
        rectTransform.sizeDelta = rectSize;
        ((RectTransform)connectionsParent).sizeDelta = rectSize;
    }

    void LayoutSocket(RectTransform socketRect, Socket data, int i)
    {
        data.setPos(positions[i]);
        slotData.Add(positions[i], new InventorySlotData(data, (RectTransform)data.transform, positions[i]));
        RectTransform socketRectChild = (RectTransform)socketRect.GetChild(0);
        socketRect.sizeDelta = Vector2.one * itemSize;
        socketRectChild.sizeDelta = Vector2.one * itemSize * .9f;
        socketRect.anchoredPosition = new Vector2(itemSize * positions[i].x + positions[i].x * spacingX, -itemSize * positions[i].y - spacingY * positions[i].y) + new Vector2(itemSize / 2, -itemSize / 2);
        Array allGemColors = Enum.GetValues(typeof(GemColor));
        data.setGemColor((GemColor)allGemColors.GetValue(UnityEngine.Random.Range(0, 3)));
    }


    protected void Awake()
    {
        belongsToItem.bindSockets(this);
        rectTransform = GetComponent<RectTransform>();
        init();

    }

    public void RerollSocketsAndConnections()
    {
        ClearSockets();
        init();
    }

    void ClearSockets()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        foreach (Transform tc in connectionsParent)
        {
            Destroy(tc.gameObject);
        }
        socketTransforms.Clear();
    }

    private void connectSockets(RectTransform child, RectTransform prevChild, int i)
    {

        var randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber > 0.5f) { return; }
        var center = (child.anchoredPosition + prevChild.anchoredPosition) / 2;
        var line = GameObject.Instantiate(connectionLinePrefab, connectionsParent);
        var lineRect = line.GetComponent<RectTransform>();
        if (i % 2 == 0)
        {
            lineRect.Rotate(new Vector3(0, 0, 90));
            lineRect.anchoredPosition = new Vector2(child.anchoredPosition.x - lineRect.rect.height / 2, center.y - lineRect.rect.width / 2);
            return;
        }
        lineRect.anchoredPosition = new Vector2(Mathf.Abs(center.x) - lineRect.rect.width / 2, child.anchoredPosition.y + lineRect.rect.height / 2);
    }

    private void makeConnections()
    {
        for (var i = 1; i < getSize(); i++)
        {
        }
    }
}
