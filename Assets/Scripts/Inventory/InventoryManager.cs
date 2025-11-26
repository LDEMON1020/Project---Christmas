using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory Setting")]
    public int inventorySize = 15;
    public GameObject inventoryUI;
    public Transform itemSlotParent;
    public GameObject itemSlotPrefab;

    [Header("Equip System")]
    public Transform weaponHolder; //  플레이어의 무기가 생성될 위치 (손)
    private GameObject currentWeaponObject; // 현재 게임 월드에 생성된 무기 오브젝트
    private Item currentEquippedItem;       // 현재 장착된 아이템 데이터

    [Header("Starting Items")]
    public List<Item> startingItems; // 게임 시작 시 지급할 아이템 목록

    [Header("Input")]
    public KeyCode inventoryKey = KeyCode.I;
    public List<InventorySlot> slots = new List<InventorySlot>();
    private bool isInventoryOpen = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        CreateInventorySlots();
        SetStartingItems(); // 시작 아이템 지급
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            ToggleInventory();
        }
    }

    // 슬롯 생성 함수
    void CreateInventorySlots()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotObject = Instantiate(itemSlotPrefab, itemSlotParent);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            slots.Add(slot);
        }
    }

    // 시작 아이템들을 인벤토리에 넣는 함수
    void SetStartingItems()
    {
        if (startingItems != null)
        {
            foreach (Item item in startingItems)
            {
                AddItem(item);
            }
        }
    }

    // 아이템 추가 (단순히 빈 슬롯을 찾아 넣음)
    public bool AddItem(Item item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null) // 빈 슬롯 발견
            {
                slot.SetItem(item);
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                slot.ClearSlot();
                // 만약 삭제된 아이템이 장착 중이던 거라면 무기도 없앰
                if (currentEquippedItem == item)
                {
                    Destroy(currentWeaponObject);
                    currentEquippedItem = null;
                }
                return;
            }
        }
    }

    //  아이템 장착 함수 
    public void EquipItem(Item newItem)
    {
        //이미 같은 걸 끼고 있다면 무시
        if (currentEquippedItem == newItem) return;

        currentEquippedItem = newItem;

        //기존에 들고 있던 무기 오브젝트 파괴
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        //새로운 무기 소환 (플레이어 손 위치에)
        if (newItem.weaponPrefab != null && weaponHolder != null)
        {
            currentWeaponObject = Instantiate(newItem.weaponPrefab, weaponHolder);
            // 위치 0으로 초기화 (손 위치에 딱 붙게)
            currentWeaponObject.transform.localPosition = Vector3.zero;
        }

        // 4. UI 업데이트 (장착된 슬롯에만 테두리 표시)
        UpdateEquipUI();
    }

    // UI 테두리 갱신용 함수
    void UpdateEquipUI()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == currentEquippedItem)
            {
                // 장착된 아이템 슬롯이면 테두리 켜기
                if (slot.selectedFrame != null) slot.selectedFrame.SetActive(true);
            }
            else
            {
                // 아니면 테두리 끄기
                if (slot.selectedFrame != null) slot.selectedFrame.SetActive(false);
            }
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UpdateEquipUI(); // 인벤 열 때 UI 상태 갱신
            Time.timeScale = 0.2f; // 인벤토리를 열었을 때 시간 느리게 하기(원하는 값으로 조정가능)
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // 인벤토리를 닫았을 때 시간을 다시 되돌리기
        }
    }
}