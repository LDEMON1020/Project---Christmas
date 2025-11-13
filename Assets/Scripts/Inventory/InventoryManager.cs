using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;   // 싱글톤 패턴

    [Header("Inventory Setting")]
    public int inventorySize = 15;              // 인벤토리 슬롯 개수
    public GameObject inventoryUI;              // 인벤토리 UI 패널
    public Transform itemSlotParent;            // 슬롯들이 들어갈 부모 오브젝트
    public GameObject itemSlotPrefab;           // 슬롯 프리팹

    [Header("Input")]
    public KeyCode inventoryKey = KeyCode.I;    // 인벤토리 열기 키
    public List<InventorySlot> slots = new List<InventorySlot>();  // 모든 슬롯 리스트
    private bool isInventoryOpen = false;       // 인벤토리가 열려있는지 확인

    void Start()
    {
        CreateInventorySlots();
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            ToggleInventory();
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool AddItem(Item item, int amount = 1)
    {
        // 1단계 : 이미 있는 아이템에 추가 시도 (스택)
        foreach (InventorySlot slot in slots)
        {
            // 같은 아이템이고, 현재 개수가 최대 스택보다 작을 때
            if (slot.item == item && slot.amount < item.maxStack)
            {
                // 남은 공간 계산
                int spaceLeft = item.maxStack - slot.amount;
                // 실제 추가할 개수 계산
                int amountToAdd = Mathf.Min(amount, spaceLeft);
                // 해당 슬롯에 개수 추가
                slot.AddAmount(amountToAdd);

                // 남은 개수 갱신
                amount -= amountToAdd;

                if (amount <= 0)
                {
                    return true;
                }
            }
        }

        // 2단계 : 빈 슬롯에 추가
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                slot.SetItem(item, amount);
                return true;
            }
        }

        Debug.Log("인벤토리가 가득 참");
        return false;
    }

    public void RemoveItem(Item item, int amount = 1) // 아이템을 인벤토리에서 제거하는 함수
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                slot.RemoveAmount(amount);
                return;
            }
        }
    }

    void CreateInventorySlots()    // 인벤토리 슬롯들을 생성하는 함수
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotObject = Instantiate(itemSlotPrefab, itemSlotParent);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            slots.Add(slot); // 리스트에 추가
        }
    }

    public void ToggleInventory()    // 인벤토리 UI를 열거나 닫는 함수
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;    // 인벤토리가 열리면 커서 보이기
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;  // 인벤토리가 닫히면 커서 숨기기
            Cursor.visible = false;
        }
    }

    public int GetItemCount(Item item)
    {
        int count = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                count += slot.amount;
            }
        }
        return count;
    }
}
