using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public int Damage = 20;            // 데미지
    public float attackRange = 1.5f;   // 레이 길이
    public float attackRate = 0.5f;    // 공격 쿨타임
    public string enemyTag = "Enemy";  // 적 태그

    private float nextAttackTime = 0f;
    private Animator animator;
    private SpriteRenderer playerSR;   // 방향 판별용

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        playerSR = GetComponentInParent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("CandyAttack");

        // 애니메이션 이벤트에서 DoRaycast() 사용해도 됨
        DoRaycast();
    }

    void DoRaycast()
    {
        // 플레이어가 바라보는 방향
        Vector2 dir = playerSR.flipX ? Vector2.left : Vector2.right;

        // 레이캐스트
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, attackRange);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag(enemyTag))
            {
                hit.collider.GetComponent<EnemyController>()?.TakeDamage(Damage);
                hit.collider.GetComponent<ReconEnemyController>()?.TakeDamage(Damage);
                hit.collider.GetComponent<RangedEnemy>()?.TakeDamage(Damage);
            }
        }
    }


}