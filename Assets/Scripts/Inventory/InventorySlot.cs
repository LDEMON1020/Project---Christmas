using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Unity 스크립트|참조 0개
public class InventorySlot : MonoBehaviour
{
    public Item item;                    // 이 슬롯에 있는 아이템
    public int amount;                       // 아이템 개수

    [Header("UI Reference")]
    public Image itemIcon;                   // 아이템 아이콘 이미지
    public Text amountText;                  // 개수 텍스트
    public GameObject emptySlotImage;        // 빈 슬롯일 때 보여줄 이미지

    void Start()
    {
        UpdateSlotUI();
    }

    // UI를 업데이트 하는 함수
    void UpdateSlotUI()
    {
        if (item != null) // 아이템이 있으면
        {
            itemIcon.sprite = item.itemIcon;   // 아이콘 표시
            itemIcon.enabled = true;

            amountText.text = amount > 1 ? amount.ToString() : "";  // 개수가 1개보다 많으면 숫자 표시

            if (emptySlotImage != null)
            {
                emptySlotImage.SetActive(false); // 빈 슬롯 이미지 숨기기
            }
        }
        else
        {
            itemIcon.enabled = false;  // 아이콘 숨기기
            amountText.text = "";      // 텍스트 비우기

            if (emptySlotImage != null)
            {
                emptySlotImage.SetActive(true); // 빈 슬롯 이미지 표시
            }
        }
    }

    public void SetItem(Item newItem, int newAmount) // 슬롯에 아이템 설정 함수
    {
        item = newItem;
        amount = newAmount;
        UpdateSlotUI();
    }

    // 아이템 개수 추가하는 함수
    public void AddAmount(int value)
    {
        amount += value;
        UpdateSlotUI();
    }

    // 아이템 개수 제거하는 함수
    public void RemoveAmount(int value)
    {
        amount -= value;

        if (amount <= 0)   // 개수가 0 이하이면 슬롯 비우기
        {
            ClearSlot();
        }
        else
        {
            UpdateSlotUI();
        }
    }

    // 슬롯을 비우는 함수
    public void ClearSlot()
    {
        item = null;
        amount = 0;
        UpdateSlotUI();
    }

}
