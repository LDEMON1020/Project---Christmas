using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public enum EnemyState { Idle, Trace, Attack }
    private EnemyState state = EnemyState.Idle;

    [Header("기본 설정")]
    public float moveSpeed = 2f;
    public float traceRange = 8f;       // 추적 거리
    public float attackRange = 5f;      // 공격 거리
    public float attackCooldown = 1.5f; // 공격 쿨타임

    [Header("투사체 관련")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("체력 관련")]
    public int maxHP = 5;
    public int currentHP;

    [Header("마나 설정")]
    public int manaReward = 10;
    private PlayerController playerController;

    private Transform player;
    private float lastAttackTime;

    private Rigidbody2D rb;
    private bool isFacingRight = true;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
       
            playerController = playerObject.GetComponent<PlayerController>();
        }

        lastAttackTime = -attackCooldown;
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 상태 전환
        switch (state)
        {
            case EnemyState.Idle:
                if (distanceToPlayer < traceRange)
                    state = EnemyState.Trace;
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;

            case EnemyState.Trace:
                if (distanceToPlayer < attackRange)
                    state = EnemyState.Attack;
                else if (distanceToPlayer > traceRange)
                    state = EnemyState.Idle;
                else
                    TracePlayer();
                break;

            case EnemyState.Attack:
                if (distanceToPlayer > attackRange)
                    state = EnemyState.Trace;
                else
                    AttackPlayer();
                rb.velocity = new Vector2(0, rb.velocity.y); // 공격 중 정지
                break;
        }
    }

    void TracePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        // 바라보는 방향 전환
        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            Flip();
    }

    void AttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();

            if (ep != null)
            {
                Vector2 dir = (player.position - firePoint.position).normalized;
                ep.SetDirection(dir);
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        // 마나 획득 로직 추가
        if (playerController != null)
        {
            // 플레이어의 RestoreMana 함수를 호출하여 마나 보상 제공
            playerController.RestoreMana(manaReward);
        }

        Destroy(gameObject);
    }
}