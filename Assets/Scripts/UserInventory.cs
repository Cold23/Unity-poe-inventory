using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlotData
{
    public InventorySlot inventorySlot;
    public RectTransform rect;
    public Vector2Int position;

    public InventorySlotData(InventorySlot slot, RectTransform rect, Vector2Int position)
    {
        this.inventorySlot = slot;
        this.rect = rect;
        this.position = position;
    }
}

public class UserInventory : Inventory
{
    RectTransform rect;
    Dictionary<Vector2Int, InventorySlotData> slotData;
    protected override void init()
    {
        slotData = new Dictionary<Vector2Int, InventorySlotData>(); ;
        rect = GetComponent<RectTransform>();
        var itemsPerRow = Mathf.FloorToInt(rect.rect.width / itemSize);
        var yIndex = 0;
        var xIndex = 0;
        for (int i = 0; i < inventorySize; i++)
        {
            var pos = new Vector2Int(xIndex, yIndex);
            var obj = GameObject.Instantiate(emptySlotPrefab, transform);
            var inventorySlot = obj.GetComponent<InventorySlot>();
            var slotRect = obj.GetComponent<RectTransform>();
            slotData.Add(pos, new InventorySlotData(inventorySlot, slotRect, pos));
            xIndex++;
            if (xIndex >= itemsPerRow)
            {
                xIndex = 0;
                yIndex++;
            }
        }
    }
    private void Awake()
    {
        init();
    }
}