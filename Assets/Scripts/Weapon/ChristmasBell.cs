using UnityEngine;

// 스턴 인터페이스 (다른 파일에도 있어도 됨. 중복되면 제거해도 됨)
public interface IStunnable
{
    void Stun(float duration);
}

public class ChristmasBell : MonoBehaviour
{
    [Header("플레이어 따라다니기")]
    public Transform target;              // 플레이어 Transform
    public float followDuration = 5f;     // 플레이어 따라다니는 시간
    public Vector3 offset = Vector3.zero; // 플레이어 기준 위치 오프셋

    [Header("기절")]
    public float stunDuration = 2f;       // 적 기절 시간
    public LayerMask enemyLayer;          // 적 레이어

    private Rigidbody2D rb;
    private float spawnTime;
    public bool destroyAfterFollowEnds = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // 플레이어를 일정 시간 따라다님
        if (Time.time - spawnTime <= followDuration)
        {
            rb.MovePosition(target.position + offset);
        }
        else
        {
            // 시간이 지나면 원 제거
            if (destroyAfterFollowEnds)
                Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // enemyLayer를 설정했다면 필터링 실행
        if (enemyLayer.value != 0 && (enemyLayer & (1 << other.gameObject.layer)) == 0)
            return;

        // 스턴 가능한 객체인지 확인
        IStunnable stunnable = other.GetComponent<IStunnable>();
        if (stunnable != null)
        {
            stunnable.Stun(stunDuration);
            return;
        }
    }
}

