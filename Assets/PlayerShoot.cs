using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject Prefab; 
    public Transform firePoint;        
    public float fireForce = 20f;      

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
            Shoot();
        }
    }

    void Shoot()
    {

        GameObject bullet = Instantiate(Prefab, firePoint.position, firePoint.rotation);

        Vector2 direction = firePoint.right;

        bullet.GetComponent<GingerCookie>().SetDirection(direction);
    }
}
