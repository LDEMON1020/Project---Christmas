using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierEnemy : MonoBehaviour
{
    public enum EnemyState { Idle, Trace, Attack }
    private EnemyState state = EnemyState.Idle;

    [Header("기본 설정")]
    public float moveSpeed = 2f;
    public float traceRange = 8f;
    public float attackRange = 6f;       // 수류탄이므로 사거리를 조금 더 길게 잡는 게 좋습니다.
    public float attackCooldown = 2.5f;  // 투척 모션 고려하여 쿨타임 증가

    [Header("수류탄 설정")]
    public GameObject grenadePrefab;     // EnemyGrenade가 붙은 프리팹
    public Transform firePoint;
    public float throwAngle = 45f;       // 던지는 각도 (45도가 가장 멀리 감)

    [Header("체력 관련")]
    public int maxHP = 5;
    public int currentHP;

    private Transform player;
    private float lastAttackTime;

    private Rigidbody2D rb;
    private bool isFacingRight = true;

    public CoinData coinData;

    private bool isKnockback = false;
    private float knockbackDuration = 0.3f;

    // 스턴 관련
    private bool isStunned = false;
    private float stunEndTime = 0f;
    public bool IsStunned => isStunned;

    private Animator animator;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        lastAttackTime = -attackCooldown;
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        if (isStunned)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isKnockback) return;
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (state)
        {
            case EnemyState.Idle:
                if (distanceToPlayer < traceRange) state = EnemyState.Trace;
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;

            case EnemyState.Trace:
                if (distanceToPlayer < attackRange) state = EnemyState.Attack;
                else if (distanceToPlayer > traceRange) state = EnemyState.Idle;
                else TracePlayer();
                break;

            case EnemyState.Attack:
                if (distanceToPlayer > attackRange) state = EnemyState.Trace;
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y); // 멈춰서 던짐
                    AttackPlayer();
                }
                break;
        }
    }

    void Update()
    {
        if (isStunned && Time.time >= stunEndTime)
        {
            isStunned = false;
        }
    }

    void TracePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            Flip();
    }

    void AttackPlayer()
    {
        if (isStunned) return;

        // 플레이어 방향을 바라보게 함
        float direction = player.position.x - transform.position.x;
        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            Flip();

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        if (grenadePrefab != null && firePoint != null)
        {
            GameObject grenadeObj = Instantiate(grenadePrefab, firePoint.position, Quaternion.identity);
            EnemyGrenade grenadeScript = grenadeObj.GetComponent<EnemyGrenade>();

            if (grenadeScript != null)
            {
                float distance = Mathf.Abs(player.position.x - firePoint.position.x);


                float gravity = Mathf.Abs(Physics2D.gravity.y);
                float rad = throwAngle * Mathf.Deg2Rad;

                float neededPower = Mathf.Sqrt((distance * gravity) / Mathf.Sin(2 * rad));

                neededPower = Mathf.Clamp(neededPower, 2f, 15f);

                grenadeScript.power = neededPower;
                grenadeScript.angle = throwAngle;
                grenadeScript.Launch(isFacingRight);
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
        if (currentHP <= 0) Die();
    }

    void Die()
    {
        if (coinData != null) DropCoin();
        Destroy(gameObject);
    }

    void DropCoin()
    {
        Instantiate(coinData.coinPrefab, transform.position, Quaternion.identity)
            .GetComponent<CoinItem>().coinData = coinData;
    }

    public void ApplyKnockback(Transform attacker, float force)
    {
        if (rb == null) return;
        isKnockback = true;
        rb.velocity = Vector2.zero;
        Vector2 direction = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        Invoke(nameof(EndKnockback), knockbackDuration);
    }

    void EndKnockback() { isKnockback = false; }

    public void Stun(float duration)
    {
        if (isStunned) return;
        isStunned = true;
        stunEndTime = Time.time + duration;
        rb.velocity = Vector2.zero;
    }
}