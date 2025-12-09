using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShooter : MonoBehaviour
{
    public GameObject prefab;
    public Transform player;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(prefab, player.position, Quaternion.identity);
        }
    }
}
