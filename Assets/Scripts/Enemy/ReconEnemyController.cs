using UnityEngine;

public class ReconEnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;        // 이동 속도
    private Rigidbody2D rb;
    private bool isFacingRight = true;  // 현재 바라보는 방향

    public int damage = 3;

    public string playerTag = "Player";

    [Header("체력 시스템")]
    public int maxHP = 20;
    public int currentHP;

    public CoinData coinData;

    private bool isKnockback = false;
    private float knockbackDuration = 0.3f;  // 넉백 유지 시간

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
        }

        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if (isDead) return;

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        if (isKnockback) return; // 넉백 중이면 이동하지 않음

        // 현재 바라보는 방향으로 계속 이동
        float moveDirection = isFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        if (isDead) return;

        // 방향 반전
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            Flip();
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetTrigger("Die");
        if (coinData != null) DropCoin();
        rb.velocity = Vector2.zero;
        isKnockback = true;
        Destroy(gameObject, 1.5f);
    }

    void DropCoin()
    {
        Debug.Log("DropCoin() 실행됨");
        GameObject coin = Instantiate(coinData.coinPrefab, transform.position, Quaternion.identity);

        coin.GetComponent<CoinItem>().coinData = coinData;
    }

    public void ApplyKnockback(Transform attacker, float force)
    {
        if (rb == null) return;

        isKnockback = true;
        rb.velocity = Vector2.zero;

        Vector2 rawDir = transform.position - attacker.position;

        // 수평 넉백만 적용
        Vector2 direction = new Vector2(rawDir.x, 0).normalized;

        rb.AddForce(direction * force, ForceMode2D.Impulse);

        Invoke(nameof(EndKnockback), knockbackDuration);
    }



    void EndKnockback()
    {
        isKnockback = false;
    }
}
