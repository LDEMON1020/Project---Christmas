using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour            //추격용 난쟁이
{
    public float moveSpeed = 2f;      // 이동 속도
    public float TraceRange = 8f;     // 추적 범위

    private Transform player;
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    [Header("체력 시스템")]
    public int maxHP = 20;
    private int currentHP;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 추적 범위 내에 있으면 계속 따라감
        if (distanceToPlayer < TraceRange)
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

            // 바라보는 방향 전환
            if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
                Flip();
        }
        else
        {
            // 너무 멀면 멈춤
            rb.velocity = new Vector2(0, rb.velocity.y);
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
        Destroy(gameObject);
    }
}