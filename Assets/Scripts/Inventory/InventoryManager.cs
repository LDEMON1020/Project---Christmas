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
    public Transform weaponHolder; // 플레이어의 무기가 생성될 위치 (손)
    private GameObject currentWeaponObject; // 현재 게임 월드에 생성된 무기 오브젝트
    private Item currentEquippedItem; // 현재 장착된 아이템 데이터
    private InventorySlot currentEquippedSlot;

    [Header("Starting Items")]
    public List<Item> startingItems; // 게임 시작 시 지급할 아이템 목록

    [Header("Input")]
    public KeyCode inventoryKey = KeyCode.I;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isInventoryOpen = false;
    public GoalObject goalObject;
    public GameObject Player;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        CreateInventorySlots();
        SetStartingItems();
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            if (goalObject.isGameClear == false)
            {
                if (Player != null)
                {
                    isInventoryOpen = !isInventoryOpen;
                    ToggleInventory();
                }
            }
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
    public bool AddItem(Item item, int count = 1)
    {
        // 1. 이미 인벤토리에 같은 '공격 시 소모' 아이템이 있는지 확인하고 스택
        if (item.consumeOnAttack)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.item == item)
                {
                    slot.UpdateCount(slot.count + count);
                    return true;
                }
            }
        }

        // 2. 빈 슬롯에 새로운 아이템으로 추가
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null) // 빈 슬롯 발견
            {
                slot.SetItem(item, count);
                return true;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
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
                    currentEquippedSlot = null;
                }
                return;
            }
        }
    }

    // 아이템 장착 함수 
    public void EquipItem(Item newItem)
    {
        //이미 같은 걸 끼고 있다면 무시
        if (currentEquippedItem == newItem) return;

        // 장착할 슬롯 찾기 및 저장
        InventorySlot slotToEquip = null;
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == newItem)
            {
                slotToEquip = slot;
                break;
            }
        }

        // 개수가 0개인 아이템은 장착 불가
        if (slotToEquip != null && slotToEquip.count <= 0)
        {
            Debug.Log("아이템 " + newItem.itemName + "의 개수가 0개이므로 장착할 수 없습니다.");
            return;
        }

        currentEquippedItem = newItem;
        currentEquippedSlot = slotToEquip; // 현재 슬롯 정보 저장

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

            inventoryUI.SetActive(false);
            Time.timeScale = 1f; // 인벤토리를 닫고 시간을 정상으로 돌리기
            isInventoryOpen = false; // 인벤토리 상태 동기화
        }

        // 4. UI 업데이트 (장착된 슬롯에만 테두리 표시)
        UpdateEquipUI();
    }

    // 공격 스크립트에서 호출해야 하는 함수
    public void ConsumeEquippedItemOnAttack()
    {
        // 1. 현재 장착된 아이템이 있고, '공격 시 소모' 유형인지 확인
        if (currentEquippedItem != null && currentEquippedItem.consumeOnAttack)
        {
            if (currentEquippedSlot != null)
            {
                // 2. 갯수 감소 처리
                currentEquippedSlot.UpdateCount(currentEquippedSlot.count - 1);

                // 3. 갯수가 0이 되면 장착 해제만 하고 슬롯은 유지
                if (currentEquippedSlot.count <= 0)
                {
                    Debug.Log(currentEquippedItem.itemName + " 아이템을 모두 소모했습니다. 장착 해제합니다. (인벤토리 슬롯은 유지)");

                    // 장착 해제
                    if (currentWeaponObject != null)
                    {
                        Destroy(currentWeaponObject);
                    }
                    currentWeaponObject = null;
                    currentEquippedItem = null;

                    // 슬롯을 비우지 않고, 레퍼런스만 초기화
                    currentEquippedSlot = null;

                    // UI 업데이트 (테두리 제거)
                    UpdateEquipUI();
                }
            }
        }
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
        inventoryUI.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UpdateEquipUI(); // 인벤 열 때 UI 상태 갱신
            Time.timeScale = 0.2f; // 인벤토리를 열었을 때 시간 느리게 하기
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // 인벤토리를 닫았을 때 시간을 다시 되돌리기
        }
    }
}