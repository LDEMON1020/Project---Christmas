using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFlipper : MonoBehaviour
{
    public SpriteRenderer sr;

    private Transform effectPoint;
    private PlayerController player;

    void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) return;

        player = playerObj.GetComponent<PlayerController>();

        Transform weapon = playerObj.transform.Find("Candy");
        if (weapon != null)
        {
            effectPoint = weapon.Find("EffectPoint");
        }
    }

    void LateUpdate()
    {
        if (sr == null || player == null || effectPoint == null) return;

        //  EffectPoint 위치로 고정
        transform.position = effectPoint.position;

        //  방향 처리
        sr.flipX = player.IsFacingLeft();
    }
}