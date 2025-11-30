using UnityEngine;

public class EnemyController : MonoBehaviour, IStunnable   //  IStunnable 추가
{
    public float moveSpeed = 2f;      
    public float TraceRange = 8f;     

    private Transform player;
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    public CoinData coinData;

    private bool isKnockback = false;
    private float knockbackDuration = 0.3f;

    [Header("체력 시스템")]
    public int maxHP = 20;
    private int currentHP;

    // 스턴 시스템 추가 
    private bool isStunned = false;
    private float stunEndTime = 0f;

    private Animator animator;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;

        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        //  스턴 중이면 완전 정지
        if (isStunned)
        {
            rb.velocity = Vector2.zero;
            return;
        }


        // 넉백 중이면 이동 중지
        if (isKnockback) return;
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < TraceRange)
        {
            float direction = player.position.x - transform.position.x;

            float flipDeadZone = 0.1f;
            if (Mathf.Abs(direction) > flipDeadZone)
            {
                rb.velocity = new Vector2(Mathf.Sign(direction) * moveSpeed, rb.velocity.y);

                if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
                    Flip();
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void Update()
    {
        //스턴 시간 체크
        if (isStunned && Time.time >= stunEndTime)
            isStunned = false;
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
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        Invoke(nameof(EndKnockback), knockbackDuration);
    }

    void EndKnockback()
    {
        isKnockback = false;
    }

    //여기서 StunCircle이 사용하는 핵심 기능
    public void Stun(float duration)
    {
        isStunned = true;
        stunEndTime = Time.time + duration;

        rb.velocity = Vector2.zero;  // 즉시 멈춤
    }
}
