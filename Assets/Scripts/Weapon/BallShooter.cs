using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooter : MonoBehaviour
{
    public GameObject BallPrefab;       // 발사체 프리팹
    public Transform firePoint;         // 발사 위치
    public float shootPower = 10f;      // 발사 속도
    public float shootAngle = 45f;      // 발사 각도

    private void Start()
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
            Shoot();
        }
    }

    void Shoot()
    {
        bool isFacingRight = transform.localScale.x > 0;

        GameObject proj = Instantiate(BallPrefab, firePoint.position, Quaternion.identity);
        BallAttack ball = proj.GetComponent<BallAttack>();
        ball.power = shootPower;
        ball.angle = shootAngle;
        ball.Launch(isFacingRight);
    }
}