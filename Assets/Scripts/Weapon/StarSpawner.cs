using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject starPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 0.3f;  // 생성 간격
    public float spawnRangeX = 5f;      // X 축 범위
    public float spawnRangeY = 3f;      // Y 축 범위

    [Header("Time Settings")]
    public float activeDuration = 3f;   // 생성 유지 시간
    public float starLifetime = 3f;     // 별 개별 유지 시간

    private float activeTimer = 0f;
    private bool isActive = true;

    void Update()
    {
        if (!isActive)
            return;

        activeTimer += Time.deltaTime;

        // 3초 후 생성 중지
        if (activeTimer >= activeDuration)
        {
            isActive = false;
            return;
        }

        // 생성 간격 마다 별 생성
        if (Time.time % spawnInterval < Time.deltaTime)
        {
            Vector2 spawnPos = new Vector2(
                transform.position.x + Random.Range(-spawnRangeX, spawnRangeX),
                transform.position.y + Random.Range(-spawnRangeY, spawnRangeY)
            );

            GameObject star = Instantiate(starPrefab, spawnPos, Quaternion.identity);

            // 생성된 별 자동 삭제
            Destroy(star, starLifetime);
        }
    }

    void OnDrawGizmosSelected()
    {
        //별 생성 범위 확인 용도 (나중에 지워도 됨)
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(spawnRangeX * 2f, spawnRangeY * 2f, 0.1f)
        );
    }
}
