using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrenade : MonoBehaviour
{
    [Header("발사체 설정")]
    public float power = 10f;
    public float angle = 45f;
    public float lifeTime = 3f;
    public int damage = 1;         // 플레이어에게 입힐 데미지 (보통 플레이어 체력은 작으므로)
    public string targetTag = "Player"; // 공격 대상 태그
    public GameObject explosionPrefab;

    // 플레이어를 감지할 레이어 (Inspector에서 Player 레이어 체크)
    public LayerMask targetLayer;

    private Rigidbody2D rb;

    public void Launch(bool isFacingRight)
    {
        rb = GetComponent<Rigidbody2D>();

        // 발사 시작 위치 보정 (바라보는 방향으로 약간 앞)
        Vector2 startOffset = isFacingRight ? Vector2.right : Vector2.left;
        transform.position += (Vector3)(startOffset * 0.3f);

        // 각도에 따른 벡터 계산
        float rad = angle * Mathf.Deg2Rad;
        Vector2 force = new Vector2(
            Mathf.Cos(rad) * power * (isFacingRight ? 1 : -1),
            Mathf.Sin(rad) * power
        );

        rb.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(ExplodeAfterDelay());
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Explode();
    }

    void Explode()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // 폭발 범위 및 데미지 처리
            CircleCollider2D col = explosion.GetComponent<CircleCollider2D>();
            if (col != null)
            {
                // 실제 폭발 반경 계산
                float realRadius = col.radius * Mathf.Max(explosion.transform.localScale.x, explosion.transform.localScale.y);

                Collider2D[] hitTargets;

                if (targetLayer != 0)
                    hitTargets = Physics2D.OverlapCircleAll(explosion.transform.position, realRadius, targetLayer);
                else
                    hitTargets = Physics2D.OverlapCircleAll(explosion.transform.position, realRadius);

                foreach (Collider2D target in hitTargets)
                {
                    if (target.CompareTag(targetTag))
                    {
                        target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            // 폭발 이펙트 제거
            Destroy(explosion, 0.5f);
        }

        // 수류탄 제거
        Destroy(gameObject);
    }
}