using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooter : MonoBehaviour
{
    public GameObject BallPrefab;         // 발사체 프리팹
    public Transform firePoint;          // 발사 위치
    public float shootPower = 10f;       // 발사 속도
    public float shootAngle = 45f;       // 발사 각도

    public int manaCost = 10;

    private Transform playerTransform;
    private PlayerController playerController;

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
            playerController = player.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckAndShoot();
        }
    }

    void CheckAndShoot()
    {
       
        if (playerController == null)
        {
            Shoot();
            return;
        }
        if (playerController.ConsumeMana(manaCost))
        {
        
            Shoot();
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