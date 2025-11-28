using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject Prefab; 
    public Transform firePoint;        
    public float fireForce = 20f;

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
            if (goalObject.isGameClear == false)
            {
                if (inventoryManager.isInventoryOpen == false)
                {
                    Shoot();
                }
            }
        }
    }

    void Shoot()
    {

        GameObject bullet = Instantiate(Prefab, firePoint.position, firePoint.rotation);

        Vector2 direction = firePoint.right;

        bullet.GetComponent<GingerCookie>().SetDirection(direction);
    }
}
