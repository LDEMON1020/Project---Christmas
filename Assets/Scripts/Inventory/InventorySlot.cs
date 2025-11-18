using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Unity 스크립트|참조 0개
public class InventorySlot : MonoBehaviour, IPointerClickHandler // 클릭 인터페이스 추가
{
    public Item item;                   // 이 슬롯에 있는 아이템

    [Header("UI Reference")]
    public Image itemIcon;              // 아이템 아이콘 이미지
    public GameObject emptySlotImage;   // 빈 슬롯일 때 보여줄 이미지
    public GameObject selectedFrame; // 장착 중임을 나타내는 테두리

    public void SetItem(Item newItem)   // 갯수 인자(int count) 삭제
    {
        item = newItem;
        UpdateSlotUI();
    }

    public void ClearSlot()
    {
        item = null;
        UpdateSlotUI();
    }

    void UpdateSlotUI()
    {
        if (item != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.enabled = true;
            if (emptySlotImage != null) emptySlotImage.SetActive(false);
        }
        else
        {
            itemIcon.enabled = false;
            if (emptySlotImage != null) emptySlotImage.SetActive(true);
        }

        // 아이템이 없거나 장착 상태가 아니면 테두리 끄기
        if (selectedFrame != null)
        {
            selectedFrame.SetActive(false);
        }
    }

    //  슬롯을 클릭했을 때 실행되는 함수
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            //아이템 장착 요청
            InventoryManager.Instance.EquipItem(item);
        }
    }
}
