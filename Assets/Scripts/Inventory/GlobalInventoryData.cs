using System.Collections.Generic;
using UnityEngine;

// Unity 에디터에서 이 Asset 파일을 생성할 수 있게 해주는 메뉴 추가
[CreateAssetMenu(fileName = "GlobalInventoryData", menuName = "Inventory/Global Inventory Data")]
public class GlobalInventoryData : ScriptableObject
{
    // 씬 이동과 관계없이 유지될 실제 데이터
    public List<InventoryItemData> items = new List<InventoryItemData>();

    // 현재 장착된 아이템의 Item 데이터 자체를 보존
    public Item currentEquippedItem;

    // 인벤토리 초기화 (게임을 새로 시작할 때 호출)
    public void Initialize(List<Item> startingItems)
    {
        items.Clear();
        currentEquippedItem = null;

        // 시작 아이템을 데이터에 채워넣음
        foreach (Item item in startingItems)
        {
            AddItem(item);
        }
    }

    // 데이터를 추가하는 로직 (AddItem과 유사하게 구현)
    public bool AddItem(Item item, int count = 1)
    {
        // 1. 이미 같은 아이템이 있는지 확인하고 스택
        if (item.consumeOnAttack)
        {
            foreach (InventoryItemData data in items)
            {
                if (data.item == item)
                {
                    data.count += count;
                    return true;
                }
            }
        }

        // 2. 새로운 아이템으로 추가 (슬롯 크기 검사는 InventoryManager에서 로직상 처리)
        items.Add(new InventoryItemData(item, count));
        return true;
    }

    // 데이터를 제거하는 로직
    public void RemoveItemData(Item item)
    {
        items.RemoveAll(data => data.item == item);
        if (currentEquippedItem == item)
        {
            currentEquippedItem = null;
        }
    }

    // 아이템 갯수 업데이트 (인벤토리 Manager에서 호출)
    public void UpdateItemCount(Item item, int newCount)
    {
        InventoryItemData data = items.Find(d => d.item == item);
        if (data != null)
        {
            data.count = newCount;
        }
    }

    // 아이템 갯수 가져오기
    public int GetItemCount(Item item)
    {
        InventoryItemData data = items.Find(d => d.item == item);
        return (data != null) ? data.count : 0;
    }
}