using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShooter : MonoBehaviour
{
    public GameObject StarPrefab;            // 발사체 프리팹
    public Transform firePoint;              // 발사 위치
    public float shootPower = 10f;           // 발사 속도
    public float shootAngle = 45f;           // 발사 각도

    private Transform playerTransform;

    [Header("발사 쿨타임 설정")]
    public float attackCooldown = 0.5f;      // 공격 사이의 최소 시간
    private float lastShootTime = 0f;        // 마지막으로 발사한 시간을 기록

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

        lastShootTime = -attackCooldown;
    }

    private void Start()
    {
        // 발사 위치(firePoint)를 찾음
        GameObject findPoint = GameObject.Find("PlayerFirePoint");

        if (findPoint != null)
        {
            firePoint = findPoint.transform;
        }

        // Player 태그를 가진 오브젝트를 찾아 방향 정보를 가져옴
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // 마우스 왼쪽 버튼을 눌렀을 때
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
        if (firePoint == null)
        {
            return;
        }

        GameObject proj = Instantiate(StarPrefab, firePoint.position, Quaternion.identity);
        StarSpawner ball = proj.GetComponent<StarSpawner>();
    }
}
