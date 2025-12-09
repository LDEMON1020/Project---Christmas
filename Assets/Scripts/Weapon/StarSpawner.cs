using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject starPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 0.3f;
    public float spawnRangeX = 5f;
    public float spawnRangeY = 3f;

    [Header("Time Settings")]
    public float activeDuration = 3f;
    public float starLifetime = 3f;

    private float activeTimer = 0f;
    private bool isActive = true;

    void Update()
    {
        if (!isActive)
            return;

        activeTimer += Time.deltaTime;

        if (activeTimer >= activeDuration)
        {
            isActive = false;

            //  생성 중지 후 StarSpawner 오브젝트 자동 파괴
            Destroy(gameObject, 0.1f);
            return;
        }

        if (Time.time % spawnInterval < Time.deltaTime)
        {
            Vector2 spawnPos = new Vector2(
                transform.position.x + Random.Range(-spawnRangeX, spawnRangeX),
                transform.position.y + Random.Range(spawnRangeY, spawnRangeY + 2)
            );

            GameObject star = Instantiate(starPrefab, spawnPos, Quaternion.identity);

            // 별 자동 삭제
            Destroy(star, starLifetime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(spawnRangeX * 2f, spawnRangeY * 2f, 0.1f)
        );
    }
}
