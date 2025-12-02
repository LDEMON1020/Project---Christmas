using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager 사용을 위해 추가

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Global Data")]
    public GlobalInventoryData globalData; // 인스펙터에서 GlobalInventoryData.asset 파일 연결 필수!
    public List<Item> startingItems; // 초기 아이템 설정용

    [Header("Inventory Setting")]
    public int inventorySize = 15;
    public GameObject inventoryUI;
    public Transform itemSlotParent;
    public GameObject itemSlotPrefab;

    [Header("Equip System")]
    public Transform weaponHolder;
    private GameObject currentWeaponObject;
    private InventorySlot currentEquippedSlot; // UI 상의 슬롯 참조

    [Header("Input")]
    public KeyCode inventoryKey = KeyCode.I;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isInventoryOpen = false;
    public GoalObject goalObject;
    public GameObject Player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad 제거. 씬이 바뀔 때마다 새로 생성됨.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 씬 로드 시마다 UI 슬롯 재생성
        CreateInventorySlots();

        // Global Data가 비어있으면 초기 아이템으로 세팅 (게임 최초 시작 시)
        if (globalData.items.Count == 0 && startingItems.Count > 0)
        {
            globalData.Initialize(startingItems);
        }

        // Global Data를 바탕으로 UI 구성
        LoadInventoryData();

        // 저장된 장착 아이템이 있다면 다시 장착
        if (globalData.currentEquippedItem != null)
        {
            // EquipItem(아이템 데이터, UI 닫기 여부)
            EquipItem(globalData.currentEquippedItem, false);
        }

        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            if (goalObject != null && goalObject.isGameClear == false)
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
        // 씬이 로드될 때마다 슬롯을 새로 생성하므로 기존 슬롯이 있다면 정리
        foreach (InventorySlot slot in slots)
        {
            if (slot != null) Destroy(slot.gameObject);
        }
        slots.Clear();

        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotObject = Instantiate(itemSlotPrefab, itemSlotParent);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            slots.Add(slot);
        }
    }

    // Global Data에서 데이터를 읽어와 UI에 표시
    void LoadInventoryData()
    {
        // 1. 모든 슬롯을 비움 (데이터는 globalData에 남아있음)
        foreach (InventorySlot slot in slots)
        {
            slot.ClearSlot();
        }

        // 2. Global Data에 저장된 아이템들을 UI 슬롯에 넣어줌
        for (int i = 0; i < globalData.items.Count && i < inventorySize; i++)
        {
            InventoryItemData data = globalData.items[i];
            slots[i].SetItem(data.item, data.count);
        }

        // 현재 장착된 슬롯을 다시 연결
        currentEquippedSlot = slots.Find(slot => slot.item == globalData.currentEquippedItem);
    }

    // 아이템 추가 (데이터 업데이트 후 UI 갱신)
    public bool AddItem(Item item, int count = 1)
    {
        // 1. Global Data에 아이템 추가
        if (globalData.AddItem(item, count))
        {
            // 2. UI 갱신 (Global Data를 다시 읽어옴)
            LoadInventoryData();
            return true;
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
        return false;
    }

    // 아이템 제거 (데이터 업데이트 후 UI 갱신 및 장착 해제)
    public void RemoveItem(Item item)
    {
        // 장착 해제 처리
        if (globalData.currentEquippedItem == item)
        {
            if (currentWeaponObject != null)
            {
                Destroy(currentWeaponObject);
            }
            globalData.currentEquippedItem = null;
            currentEquippedSlot = null;
        }

        // Global Data에서 제거
        globalData.RemoveItemData(item);

        // UI 갱신
        LoadInventoryData();
        UpdateEquipUI();
    }

    // 아이템 장착 함수
    public void EquipItem(Item newItem)
    {
        EquipItem(newItem, true); // 기본적으로 인벤토리 닫기
    }

    // 오버로드: 씬 로드 시 EquipItem 호출용
    public void EquipItem(Item newItem, bool closeInventory)
    {
        //이미 같은 걸 끼고 있다면 무시
        if (globalData.currentEquippedItem == newItem && currentWeaponObject != null) return;

        // 개수가 0개인 아이템은 장착 불가
        if (globalData.GetItemCount(newItem) <= 0)
        {
            Debug.Log("아이템 " + newItem.itemName + "의 개수가 0개이므로 장착할 수 없습니다.");
            return;
        }

        // Global Data에 장착 정보 저장
        globalData.currentEquippedItem = newItem;
        currentEquippedSlot = slots.Find(slot => slot.item == newItem); // UI 슬롯 참조 업데이트

        //기존에 들고 있던 무기 오브젝트 파괴
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        //새로운 무기 소환 (플레이어 손 위치에)
        if (newItem.weaponPrefab != null && weaponHolder != null)
        {
            currentWeaponObject = Instantiate(newItem.weaponPrefab, weaponHolder);
            currentWeaponObject.transform.localPosition = Vector3.zero;

            if (closeInventory)
            {
                inventoryUI.SetActive(false);
                Time.timeScale = 1f;
                isInventoryOpen = false;
            }
        }

        // UI 업데이트 (장착된 슬롯에만 테두리 표시)
        UpdateEquipUI();
    }

    // 공격 스크립트에서 호출해야 하는 함수
    public void ConsumeEquippedItemOnAttack()
    {
        Item equippedItem = globalData.currentEquippedItem;

        if (equippedItem != null && equippedItem.consumeOnAttack)
        {
            int currentCount = globalData.GetItemCount(equippedItem);

            if (currentCount > 0)
            {
                // 갯수 감소 처리 (Global Data 업데이트)
                globalData.UpdateItemCount(equippedItem, currentCount - 1);

                // UI 업데이트
                if (currentEquippedSlot != null)
                {
                    currentEquippedSlot.UpdateCount(currentCount - 1);
                }

                // 갯수가 0이 되면 장착 해제
                if (currentCount - 1 <= 0)
                {
                    if (currentWeaponObject != null)
                    {
                        Destroy(currentWeaponObject);
                    }
                    currentWeaponObject = null;
                    globalData.currentEquippedItem = null;
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
            if (slot.item == globalData.currentEquippedItem) // Global Data 참조
            {
                if (slot.selectedFrame != null) slot.selectedFrame.SetActive(true);
            }
            else
            {
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
            UpdateEquipUI();
            Time.timeScale = 0.2f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }
}