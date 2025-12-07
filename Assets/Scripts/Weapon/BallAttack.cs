using System.Collections;
using UnityEngine;

public class BallAttack : MonoBehaviour
{
    [Header("발사체 설정")]
    public float power = 10f;
    public float angle = 45f;
    public float lifeTime = 3f;
    public int damage = 20;
    public string enemyTag = "Enemy";
    public GameObject explosionPrefab;

    // 레이어 마스크 추가 (적만 정확히 골라내기 위해)
    public LayerMask enemyLayer;

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

        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        CircleCollider2D col = explosion.GetComponent<CircleCollider2D>();

        if (col != null)
        {
            col.isTrigger = true;

            float realRadius = col.radius * Mathf.Max(explosion.transform.localScale.x, explosion.transform.localScale.y);

            Collider2D[] hitEnemies;

            if (enemyLayer != 0)
                hitEnemies = Physics2D.OverlapCircleAll(explosion.transform.position, realRadius, enemyLayer);
            else
                hitEnemies = Physics2D.OverlapCircleAll(explosion.transform.position, realRadius);

            foreach (Collider2D enemy in hitEnemies)
            {
                // 태그 확인 
                if (enemy.CompareTag(enemyTag))
                {
                    enemy.GetComponent<EnemyController>()?.TakeDamage(damage);
                    enemy.GetComponent<RangedEnemy>()?.TakeDamage(damage);
                    enemy.GetComponent<ReconEnemyController>()?.TakeDamage(damage);
                    enemy.GetComponent<GrenadierEnemy>()?.TakeDamage(damage);
                }
            }
        }

        // 폭발 프리팹 삭제 및 자기 자신 삭제
        Destroy(explosion, 0.5f);
        Destroy(gameObject);
    }

}