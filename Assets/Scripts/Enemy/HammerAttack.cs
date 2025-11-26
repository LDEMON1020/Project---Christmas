using System.Collections;
using UnityEngine;

public class HammerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public int damage = 5;
    public float attackRange = 1.2f;      // 공격 거리
    public float attackCooldown = 1f;     // 공격 쿨타임
    public float attackDelay = 0.2f;      // 공격 타격 타이밍

    [Header("애니메이션")]
    public Animator animator;

    [Header("태그 설정")]
    public string playerTag = "Player";   // 공격 대상 태그

    private Transform enemy;
    private Transform player;
    private bool isAttacking = false;

    void Start()
    {
        enemy = transform.parent;
        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null || isAttacking) return;

        float dist = Vector2.Distance(enemy.position, player.position);

        // 공격 범위 안이면 공격
        if (dist <= attackRange)
        {
            StartCoroutine(AttackRoutine());
        }

        // 바라보는 방향 전환
        if (player.position.x < enemy.position.x)
            enemy.localScale = new Vector3(-1, 1, 1);
        else
            enemy.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 공격 애니메이션
        if (animator != null)
            animator.SetTrigger("HammerAttack");

        // 공격 타이밍 딜레이
        yield return new WaitForSeconds(attackDelay);

        DoAttackRaycast();

        // 쿨타임
        yield return new WaitForSeconds(attackCooldown - attackDelay);
        isAttacking = false;
    }

    void DoAttackRaycast()
    {
        Vector2 dir = enemy.localScale.x > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, attackRange);
        if (hit.collider != null && hit.collider.CompareTag(playerTag))
        {
            PlayerController pc = hit.collider.GetComponent<PlayerController>();
            if (pc != null)
                pc.TakeDamage(damage);
        }
    }
}