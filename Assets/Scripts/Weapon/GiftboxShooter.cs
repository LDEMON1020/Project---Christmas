using UnityEngine;

public class GiftboxShooter : MonoBehaviour
{
    public GameObject GiftboxPrefab;        // 발사할 Giftbox 프리팹
    public Transform firePoint;             // 발사 위치
    public float shootPower = 10f;          // 발사 속도
    public float shootAngle = 45f;          // 발사 각도 (수평을 기준으로)

    private Transform playerTransform;

    [Header("발사 쿨타임 설정")]
    public float attackCooldown = 0.5f;     // 공격 사이의 최소 시간
    private float lastShootTime = 0f;       // 마지막으로 발사한 시간을 기록

    [Header("기타 참조")]
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

        // 게임 시작 시 바로 발사 가능하도록 초기화
        lastShootTime = -attackCooldown;
    }

    private void Start()
    {
        GameObject findPoint = GameObject.Find("PlayerFirePoint");
        if (findPoint != null)
        {
            firePoint = findPoint.transform;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= lastShootTime + attackCooldown)
            {
                if (goalObject != null && goalObject.isGameClear == false)
                {
                    if (inventoryManager != null && inventoryManager.isInventoryOpen == false)
                    {
                        Shoot();
                        lastShootTime = Time.time;

                        if (InventoryManager.Instance != null)
                        {
                            InventoryManager.Instance.ConsumeEquippedItemOnAttack();
                        }
                    }
                }
            }
        }
    }

    void Shoot()
    {
        bool isFacingRight;
        if (playerTransform != null)
        {
            isFacingRight = playerTransform.localScale.x > 0;
        }
        else
        {
            isFacingRight = transform.localScale.x > 0;
        }

        if (firePoint == null || GiftboxPrefab == null)
        {
            if (firePoint == null) Debug.LogError("FirePoint가 설정되지 않았습니다.");
            if (GiftboxPrefab == null) Debug.LogError("GiftboxPrefab이 설정되지 않았습니다.");
            return;
        }

        GameObject proj = Instantiate(GiftboxPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            float horizontalDirection = isFacingRight ? 1f : -1f;

            Vector2 direction = Vector2.right * horizontalDirection;

            Quaternion rotation = Quaternion.Euler(0, 0, shootAngle * horizontalDirection);
            Vector2 launchVector = rotation * direction;
            rb.AddForce(launchVector.normalized * shootPower, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("발사된 Giftbox 프리팹에 Rigidbody2D 컴포넌트가 없습니다! 발사할 수 없습니다.");
            Destroy(proj);
        }
    }
}