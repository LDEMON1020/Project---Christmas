using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocoa : MonoBehaviour
{
    public int healAmount = 5;          // 회복량
    public float cooldownTime = 60f;    // 1분 쿨타임
    public Animator animator;
    public AudioSource audioSource;

    private bool isAvailable = true;
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))   // 마우스 좌클릭
        {
            TryUse();
        }
    }

    void TryUse()
    {
        if (!isAvailable) return;
        if (player == null) return;

        // 회복 적용
        player.Heal(healAmount);

        // 아이템 사용 애니메이션
        if (animator != null)
            animator.SetTrigger("Use");

        // 회복 사운드 시작
        audioSource.Play();

        // 아이템 소모
        InventoryManager.Instance.ConsumeEquippedItemOnAttack();

        // 쿨타임 시작
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        isAvailable = false;

        // (선택) 쿨타임 동안 아이템 비활성화 연출 가능

        yield return new WaitForSeconds(cooldownTime);

        isAvailable = true;
    }
}