using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public int Damage = 20;       // 공격력
    public float attackRate = 0.5f;     // 공격 속도 (쿨타임)
    public string enemyTag = "Enemy";   // 공격할 대상 태그

    private float nextAttackTime = 0f;
    private Animator animator;
    private Collider2D candyCollider;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        candyCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackRate;
        }

        FlipCandy();
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("CandyAttack");
        // 애니메이션 이벤트에서 EnableWeaponCollider/DisableWeaponCollider 호출
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            collision.GetComponent<ReconEnemyController>()?.TakeDamage(Damage);
            collision.GetComponent<EnemyController>()?.TakeDamage(Damage);
            collision.GetComponent<RangedEnemy>()?.TakeDamage(Damage);
        }
    }
    public void EnableWeaponCollider()
    {
        candyCollider.isTrigger = true;
    }

    public void DisableWeaponCollider()
    {
        candyCollider.isTrigger = false;
    }

    void FlipCandy()
    {
    SpriteRenderer playerSR = GetComponentInParent<SpriteRenderer>();
    if (playerSR == null) return;

    Vector3 scale = transform.localScale;

    if (playerSR.flipX) // 플레이어가 왼쪽을 바라볼 때
    {
        scale.x = -Mathf.Abs(scale.x);      // 무기 스프라이트 좌우 반전
        transform.localPosition = new Vector3(-Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
    }
    else // 오른쪽
    {
        scale.x = Mathf.Abs(scale.x);
        transform.localPosition = new Vector3(Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
    }

    transform.localScale = scale;
    }
}