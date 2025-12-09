using UnityEngine;

public class CandyAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public int Damage = 20;            // 데미지
    public float attackRate = 0.5f;    // 공격 쿨타임
    public string enemyTag = "Enemy";  // 적 태그

    [Header("공격 범위 설정")]
    public Vector2 boxSize = new Vector2(1.5f, 1f); // 공격 범위 크기

    private float nextAttackTime = 0f;
    private Animator animator;
    private SpriteRenderer playerSR;   // 방향 판별용

    [Header("시각 효과")]
    public GameObject attackEffectPrefab;
    [Header("기타 참조")]
    public InventoryManager inventoryManager;
    public GoalObject goalObject;
    [Header("공격 위치")]
    public Transform attackPoint;

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

        attackPoint = transform.Find("EffectPoint");
        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint 를 찾을 수 없습니다 (무기 자식으로 필요)");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            if (goalObject.isGameClear == false)
            {
                if (inventoryManager.isInventoryOpen == false)
                {
                    Attack();
                    nextAttackTime = Time.time + attackRate;
                }
            }
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("CandyAttack");

        DoBoxAttack();
    }

    void DoBoxAttack()
    {
        Vector2 dir = playerSR.flipX ? Vector2.left : Vector2.right;

        Vector2 origin = attackPoint != null
            ? (Vector2)attackPoint.position
            : (Vector2)transform.position;

        Vector2 boxCenter = origin + dir * (boxSize.x / 2f);

        // 이펙트 생성
        if (attackEffectPrefab != null && attackPoint != null)
        {
            GameObject effect = Instantiate(
                attackEffectPrefab,
                attackPoint.position,
                Quaternion.identity,
                attackPoint
            );

            // 방향 반전만 처리
            float directionScale = playerSR.flipX ? -1f : 1f;
            effect.transform.localScale = new Vector3(
                directionScale,
                1f,
                1f
            );

            Destroy(effect, 0.6f);
        }

        // 공격 판정은 그대로 boxCenter 사용
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);
        foreach (Collider2D col in hits)
        {
            if (col.CompareTag(enemyTag))
            {
                col.GetComponent<EnemyController>()?.TakeDamage(Damage);
                col.GetComponent<ReconEnemyController>()?.TakeDamage(Damage);
                col.GetComponent<RangedEnemy>()?.TakeDamage(Damage);
                col.GetComponent<GrenadierEnemy>()?.TakeDamage(Damage);
            }
        }
    }
   
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Vector2 dir = playerSR != null && playerSR.flipX ? Vector2.left : Vector2.right;
        Vector2 boxCenter = (Vector2)attackPoint.position + dir * (boxSize.x / 2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}