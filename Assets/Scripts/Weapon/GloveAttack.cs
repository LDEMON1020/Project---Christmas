using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public int Damage = 15;
    public float attackRange = 1.5f;
    public float attackRate = 0.5f;
    public string enemyTag = "Enemy";

    [Header("넉백 설정")]
    public float knockbackForce = 5f;

    [Header("애니메이션 소모 딜레이")]
    // 이 값을 공격 애니메이션의 길이(초)로 설정해야 합니다.
    public float consumeDelay = 0.3f;

    private float nextAttackTime = 0f;
    private Animator animator;
    private SpriteRenderer playerSR;

    [Header("시각 효과")]
    public GameObject attackEffectPrefab;

    [Header("참조")]
    public InventoryManager inventoryManager;
    public GoalObject goalObject;

    void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        goalObject = FindObjectOfType<GoalObject>();

        if (inventoryManager == null)
        {
            Debug.LogError("Inventory Manager 컴포넌트(타입: " + typeof(InventoryManager).Name + ")를 씬에서 찾을 수 없습니다.");
        }
    }

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        playerSR = GetComponentInParent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            if (goalObject.isGameClear == false)
            {
                if (inventoryManager.isInventoryOpen == false)
                {
                    // 코루틴으로 공격 시작
                    StartCoroutine(AttackCoroutine());
                    nextAttackTime = Time.time + attackRate;
                }
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        Attack();
        yield return new WaitForSeconds(consumeDelay);

        InventoryManager.Instance.ConsumeEquippedItemOnAttack();
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("GloveAttack");

        DoRaycast();
    }

    void DoRaycast()
    {
        Vector2 dir = playerSR.flipX ? Vector2.left : Vector2.right;

        // 공격 이펙트
        if (attackEffectPrefab != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, (Vector2)transform.position + dir * attackRange / 2, Quaternion.identity);
            effect.transform.localScale = new Vector3(attackRange, 1f, 1f);
            Destroy(effect, 0.2f);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, attackRange);

        if (hit.collider != null && hit.collider.CompareTag(enemyTag))
        {
            Vector2 knockDir = dir;

            // 데미지
            hit.collider.GetComponent<EnemyController>()?.TakeDamage(Damage);
            hit.collider.GetComponent<EnemyController>()?.ApplyKnockback(transform, knockbackForce);

            hit.collider.GetComponent<ReconEnemyController>()?.TakeDamage(Damage);
            hit.collider.GetComponent<ReconEnemyController>()?.ApplyKnockback(transform, knockbackForce);

            hit.collider.GetComponent<RangedEnemy>()?.TakeDamage(Damage);
            hit.collider.GetComponent<RangedEnemy>()?.ApplyKnockback(transform, knockbackForce);

            hit.collider.GetComponent<GrenadierEnemy>()?.TakeDamage(Damage);
            hit.collider.GetComponent<GrenadierEnemy>()?.ApplyKnockback(transform, knockbackForce);

        }
    }
}