using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour
{
    [Header("발사체 설정")]
    public float power = 10f;         // 발사 힘
    public float angle = 45f;         // 발사 각도 (degree)
    public float lifeTime = 3f;       // 자동 삭제 시간
    public int damage = 20;
    public string enemyTag = "Enemy";

    private Rigidbody2D rb;

    // 발사 함수
    public void Launch(bool isFacingRight)
    {
        rb = GetComponent<Rigidbody2D>();

        // 플레이어와 겹치지 않게 Spawn 위치 조정
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        transform.position += (Vector3)(direction * 0.6f);

        // 각도를 라디안으로 변환
        float rad = angle * Mathf.Deg2Rad;

        // x, y 힘 계산
        Vector2 force = new Vector2(Mathf.Cos(rad) * power * (isFacingRight ? 1 : -1),
                                    Mathf.Sin(rad) * power);

        rb.AddForce(force, ForceMode2D.Impulse);

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(enemyTag))
        {
            collision.collider.GetComponent<EnemyController>()?.TakeDamage(damage);
            collision.collider.GetComponent<RangedEnemy>()?.TakeDamage(damage);
            collision.collider.GetComponent<ReconEnemyController>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}