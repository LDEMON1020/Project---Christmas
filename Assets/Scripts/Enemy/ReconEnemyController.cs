using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconEnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;        // 이동 속도
    private Rigidbody2D rb;
    private bool isFacingRight = true;  // 현재 바라보는 방향

    [Header("체력 시스템")]
    public int maxHP = 20;
    public int currentHP;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

    void FixedUpdate()
    {
        // 현재 바라보는 방향으로 계속 이동
        float moveDirection = isFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        // 방향 반전
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // "Wall" 태그를 가진 오브젝트에 부딪히면 방향 반전
        if (collision.gameObject.CompareTag("Boundary"))
        {
            Flip();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
