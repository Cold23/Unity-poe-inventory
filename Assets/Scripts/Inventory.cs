using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected int inventorySize = 60;
    [SerializeField] protected GameObject emptySlotPrefab;
    [SerializeField] protected int itemSize = 60;
    protected void setSize(int size)
    {
        this.inventorySize = size;
    }

    protected int getSize()
    {
        return inventorySize;
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


