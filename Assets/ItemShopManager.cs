using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemShopManager : MonoBehaviour
{  
    public CoinManager coinManager;

    private void Awake()
    {
        coinManager = FindObjectOfType<CoinManager>();
}
    public void BuyItem(Item itemToBuy)
    {
       if(itemToBuy.Price <= coinManager.coin)
        {
            InventoryManager.Instance.AddItem(itemToBuy, 1);
            coinManager.coin -= itemToBuy.Price;
        }
       else
        {
            Debug.Log("Not enough coins to buy the item.");
        }
    }
}
