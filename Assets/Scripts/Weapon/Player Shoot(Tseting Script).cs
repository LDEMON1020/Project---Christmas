using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject Prefab; // 발사할 프리팹
    public Transform firePoint;         // 총알이 나가는 위치
    public float fireForce = 20f;       // 초기 발사 속도

    void Start()
    {
        //발사 위치(firePoint)를 찾음
        GameObject findPoint = GameObject.Find("PlayerFirePoint");

        if (findPoint != null)
        {
            firePoint = findPoint.transform;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭 = 발사
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 프리팹 생성
        GameObject bullet = Instantiate(Prefab, firePoint.position, firePoint.rotation);

        // 방향(플레이어가 바라보는 방향)
        Vector2 direction = firePoint.right;

        // Projectile 스크립트에 방향 전달
        bullet.GetComponent<GingerCookie>().SetDirection(direction);
    }
}
