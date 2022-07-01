using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Inventory base class
/// </summary>
public class Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected int inventorySize = 60;
    [SerializeField] protected GameObject emptySlotPrefab;
    [SerializeField] protected int itemSize = 60;
    protected Dictionary<Vector2Int, InventorySlotData> slotData;

    protected void setSize(int size)
    {
        this.inventorySize = size;
    }

    public void setSlotItem(Vector2Int pos, UIItem item)
    {
        slotData[pos].inventorySlot.setSlotItem(item);
    }

    public void removeSlotItem(Vector2Int pos)
    {
        slotData[pos].inventorySlot.setSlotItem(null);

    }

    protected int getSize()
    {
        return inventorySize;
    }

    public virtual InventorySlotData getSlotItem(Vector2Int pos)
    {
        slotData.TryGetValue(pos, out var slot);
        return slot;
    }

    protected virtual void init()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject.Instantiate(emptySlotPrefab, transform);
        }
    }

    public virtual void onDropItem()
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseObject.setInventory(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseObject.clearInventory();
    }
}


