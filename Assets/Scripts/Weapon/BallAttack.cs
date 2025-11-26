using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour
{
    [Header("발사체 설정")]
    public float power = 10f;              // 발사 힘
    public float angle = 45f;              // 발사 각도
    public float lifeTime = 3f;            // 폭발까지 시간
    public int damage = 20;
    public string enemyTag = "Enemy";
    public GameObject explosionPrefab;     // 폭발 범위 프리팹

    private Rigidbody2D rb;

    public void Launch(bool isFacingRight)
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        transform.position += (Vector3)(direction * 0.6f);

        float rad = angle * Mathf.Deg2Rad;
        Vector2 force = new Vector2(Mathf.Cos(rad) * power * (isFacingRight ? 1 : -1),
                                    Mathf.Sin(rad) * power);

        rb.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(ExplodeAfterDelay());
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(lifeTime);

        // 폭발 프리팹 생성
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // 폭발 프리팹의 Collider를 트리거로 강제 설정
        CircleCollider2D col = explosion.GetComponent<CircleCollider2D>();
        if (col != null) col.isTrigger = true;

        // 트리거 범위 안 적 체크
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            explosion.transform.position,
            col.radius
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag(enemyTag))
            {
                enemy.GetComponent<EnemyController>()?.TakeDamage(damage);
                enemy.GetComponent<RangedEnemy>()?.TakeDamage(damage);
                enemy.GetComponent<ReconEnemyController>()?.TakeDamage(damage);
            }
        }

        // 폭발 프리팹 잠시 후 삭제
        Destroy(explosion, 0.5f);
        Destroy(gameObject);
    }
}