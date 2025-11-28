using UnityEngine;

public class EnemyController : MonoBehaviour            //추격용 난쟁이
{
    public float moveSpeed = 2f;      // 이동 속도
    public float TraceRange = 8f;     // 추적 범위

    private Transform player;
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    public CoinData coinData;

    private bool isKnockback = false;
    private float knockbackDuration = 0.3f;  // 넉백 유지 시간

    [Header("체력 시스템")]
    public int maxHP = 20;
    private int currentHP;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
      
        }

        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

    void FixedUpdate()
    {
        if (isKnockback) return; // 넉백 중이면 이동하지 않음

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < TraceRange)
        {
            float direction = player.position.x - transform.position.x;

            // 플레이어가 너무 가까우면 방향 전환 X
            float flipDeadZone = 0.1f; // 이 범위 내면 방향 안 바꿈
            if (Mathf.Abs(direction) > flipDeadZone)
            {
                rb.velocity = new Vector2(Mathf.Sign(direction) * moveSpeed, rb.velocity.y);

                if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
                    Flip();
            }
            else
            {
                // 너무 가까우면 멈춤
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        DropCoin();
        Destroy(gameObject);
    }

    void DropCoin()
    {
        GameObject coin = Instantiate(coinData.coinPrefab, transform.position, Quaternion.identity);

        coin.GetComponent<CoinItem>().coinData = coinData;
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb == null) return;

        isKnockback = true;
        rb.velocity = Vector2.zero; // 기존 이동 초기화
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        Invoke(nameof(EndKnockback), knockbackDuration);
    }

    void EndKnockback()
    {
        isKnockback = false;
    }
}