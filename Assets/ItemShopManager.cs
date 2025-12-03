using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemShopManager : MonoBehaviour
{
    
    public CoinManager coinManager;
    public int itemPrice = 100;

    private void Awake()
    {
        coinManager = FindObjectOfType<CoinManager>();
}
    public void BuyItem(Item itemToBuy)
    {
       if(itemPrice <= coinManager.coin)
        {
            InventoryManager.Instance.AddItem(itemToBuy, 1);
            coinManager.coin -= itemPrice;
        }
       else
        {
            Debug.Log("Not enough coins to buy the item.");
        }
    }
}
