using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasBellShoot : MonoBehaviour
{
        public GameObject Prefab;
        public Transform firePoint;
        public float fireForce = 20f;

        [Header("발사 쿨타임 설정")]
        public float attackCooldown = 0.5f;
        private float lastShootTime = 0f;

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

        void Start()
        {
            GameObject findPoint = GameObject.Find("PlayerFirePoint");

            if (findPoint != null)
            {
                firePoint = findPoint.transform;
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
            if (firePoint == null) return;

            GameObject bullet = Instantiate(Prefab, firePoint.position, firePoint.rotation);

            // 1) 방향 설정 (기존 코드)
            Vector2 direction = firePoint.right;
            GingerCookie cookie = bullet.GetComponent<GingerCookie>();
            if (cookie != null)
                cookie.SetDirection(direction);

            // 2) ChristmasBell 타겟 설정 추가
            ChristmasBell bell = bullet.GetComponent<ChristmasBell>();
        if (bell != null)
        {
            bell.SetTarget(this.transform);
        }
    }
    }

