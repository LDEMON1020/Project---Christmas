using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giftbox : MonoBehaviour
{
    public GameObject shardPrefab;
    public int minShards = 2;
    public int maxShards = 3;

    void Start()
    {
        Explode();
    }

    public void Explode()
    {
        int shardCount = Random.Range(minShards, maxShards + 1);

        for (int i = 0; i < shardCount; i++)
        {
            GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);

            // 위쪽(Up)을 기준으로 -90° ~ +90° 범위
            float angle = Random.Range(-90f, 90f);

            // 기준 방향: 위쪽 (0, 1)
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

            Rigidbody2D rb = shard.GetComponent<Rigidbody2D>();
            rb.AddForce(direction.normalized * Random.Range(6f, 10f), ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }
}
