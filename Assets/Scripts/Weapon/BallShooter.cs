using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooter : MonoBehaviour
{
    public GameObject BallPrefab;       // 발사체 prefab
    public Transform firePoint;         // 발사 위치

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
        proj.GetComponent<BallAttack>().Launch(isFacingRight);
    }
}
