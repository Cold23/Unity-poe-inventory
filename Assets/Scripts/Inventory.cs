using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int size = 60;
    [SerializeField] protected GameObject emptySlotPrefab;

    protected void setSize(int size)
    {
        this.size = size;
    }

    protected int getSize()
    {
        return size;
    }

    protected virtual void init()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject.Instantiate(emptySlotPrefab, transform);
        }
    }
}


