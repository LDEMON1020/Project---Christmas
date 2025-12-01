using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemButton : MonoBehaviour
{
    public Item itemToGrant;

    public void GrantItem()
    {
        if (itemToGrant != null)
        {
            bool success = InventoryManager.Instance.AddItem(itemToGrant, 1);

            if (success)
            {
                Debug.Log(itemToGrant.itemName + " 1개를 획득했습니다.");
            }
            else
            {
                Debug.Log("인벤토리가 가득 차서 " + itemToGrant.itemName + "을 획득할 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("할당된 획득 아이템이 없습니다.");
        }
    }
}