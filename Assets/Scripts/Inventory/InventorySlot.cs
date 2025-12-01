using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public int count;

    [Header("UI Reference")]
    public Image itemIcon;
    public GameObject emptySlotImage;
    public GameObject selectedFrame;
    public Text countText;

    public void SetItem(Item newItem, int newCount = 1)
    {
        item = newItem;
        count = newCount;
        UpdateSlotUI();
    }

    public void UpdateCount(int newCount)
    {
        count = newCount;
        UpdateSlotUI();
    }

    public void ClearSlot()
    {
        item = null;
        count = 0;
        UpdateSlotUI();
    }

    void UpdateSlotUI()
    {
        if (item != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.enabled = true;
            if (emptySlotImage != null) emptySlotImage.SetActive(false);

            if (countText != null)
            {
                countText.text = count > 1 ? count.ToString() : "";
            }
        }
        else
        {
            itemIcon.enabled = false;
            if (emptySlotImage != null) emptySlotImage.SetActive(true);
            if (countText != null) countText.text = "";
        }

        // 아이템이 없거나 장착 상태가 아니면 테두리 끄기
        if (selectedFrame != null)
        {
            selectedFrame.SetActive(false);
        }
    }

    // 슬롯을 클릭했을 때 실행되는 함수
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            if (item.isEquippable)
            {
                //아이템 장착 요청
                InventoryManager.Instance.EquipItem(item);
            }
        }
    }
}