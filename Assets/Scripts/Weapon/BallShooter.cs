using UnityEngine;

public class BallShooter : MonoBehaviour
{
    public GameObject BallPrefab;         // 발사체 프리팹
    public Transform firePoint;          // 발사 위치
    public float shootPower = 10f;       // 발사 속도
    public float shootAngle = 45f;       // 발사 각도

    private Transform playerTransform;


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
    }

    private void Start()
    {
        //발사 위치(firePoint)를 찾음
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
        if (Input.GetMouseButtonDown(0))
        {
            if (goalObject.isGameClear == false)
            {
                if (inventoryManager.isInventoryOpen == false)
                {
                    Shoot();
                    InventoryManager.Instance.ConsumeEquippedItemOnAttack();
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

        GameObject proj = Instantiate(BallPrefab, firePoint.position, Quaternion.identity);
        BallAttack ball = proj.GetComponent<BallAttack>();

        if (ball != null)
        {
            ball.power = shootPower;
            ball.angle = shootAngle;
            ball.Launch(isFacingRight);
        }
        else
        {
            Destroy(proj); 
        }


    }
}