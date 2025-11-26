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

    [Header("시각 효과")]
    public GameObject attackEffectPrefab;

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
        Vector2 dir = playerSR.flipX ? Vector2.left : Vector2.right;

        // 범위 이펙트 생성
        if (attackEffectPrefab != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, (Vector2)transform.position + dir * attackRange / 2, Quaternion.identity);
            effect.transform.localScale = new Vector3(attackRange, 1f, 1f); // 범위에 맞춰 크기 조절
            Destroy(effect, 0.2f); // 0.2초 후 사라짐
        }

        // 실제 공격
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