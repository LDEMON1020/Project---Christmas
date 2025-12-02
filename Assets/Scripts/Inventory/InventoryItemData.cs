using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    // InventoryManager.cs에서 사용하고 있는 Item 클래스를 그대로 사용
    public Item item;
    public int count;

    public InventoryItemData(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }
}