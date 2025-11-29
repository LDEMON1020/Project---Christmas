using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("플레이어 움직임")]
    public float moveSpeed = 5f;            //좌우 이동 속도
    public float jumpForce = 7f;            //점프 힘
    private bool isGrounded = false;        //땅에 닿아있는지 여부

    [Header("플레이어 체력 시스템")]
    public int maxHP = 20;
    private int currentHP;
    public Slider hpSlider;

    [Header("크리스마스 벨 시스템")]
    public GameObject stunCirclePrefab;  // Bell Circle 프리팹
    public float stunCircleDuration = 5f;
    public float stunDuration = 2f;


    private Rigidbody2D rb;

    [Header("플레이어 스프라이트")] // 위의 헤더하고 같이 나와서 구분 하는 용도
    public SpriteRenderer candySr;
    public SpriteRenderer sr;
  
    [Header("땅 체크")]
    public Transform groundCheck;           //땅 체크용
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;           //ground 레이어 설정 필요

    public CandyAttack candyAttack;

    public GameObject gameOverPanel;        //게임 오버 판넬

    private bool isInvincible; // 무적 상태 여부

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHP = maxHP;
        hpSlider.value = 1f;
    }

 
    void Update()
    {
        Move();
        Jump();
        Run();
    }
    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

    }

        void Jump()
    {
        // Ground 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 점프 입력 (W or Space)
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }



    public void TakeDamage(int damage)
    {
        if (isInvincible) return;  // 무적이면 데미지 안 받음

        currentHP -= damage;
        hpSlider.value = (float)currentHP / maxHP;

        // 데미지 입었을 때 깜빡이는 코루틴 실행
        if (currentHP > 0) // 죽지 않았을 때만 효과 실행
        {
            StartCoroutine(DamageFlash());
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }


    // 데미지 플래시 효과 코루틴
    IEnumerator DamageFlash()
    {
        //반투명 빨간색으로 변경
        sr.color = new Color(1f, 0f, 0f, 0.5f);

        //0.2초 대기 (깜빡이는 시간)
        yield return new WaitForSeconds(0.2f);

        //원래 색(흰색)으로 복귀
        sr.color = Color.white;
    }

   public void Die()
   {
        Destroy(gameObject);
        gameOverPanel.SetActive(true);
   }


    public void Run()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 10f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cake"))
        {
            // 3초 동안 무적
            StartCoroutine(InvincibleTime(3f));

            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Bell"))
        {
            SpawnStunCircle();
            Destroy(collision.gameObject); // 아이템 삭제
        }
    }

    public IEnumerator InvincibleTime(float duration)
    {
        isInvincible = true;
        
        yield return new WaitForSeconds(duration);

        isInvincible = false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;

        hpSlider.value = (float)currentHP / maxHP;

        StartCoroutine(HealEffectCoroutine());
    }

    IEnumerator HealEffectCoroutine()
    {
        Color originalColor = sr.color;
        Color healColor = new Color(0.3f, 1f, 0.3f, 0.5f);

        int blinkCount = 6;
        float blinkTime = 0.1f;

        for (int i = 0; i < blinkCount; i++)
        {
            sr.color = healColor;
            yield return new WaitForSeconds(blinkTime);

            sr.color = originalColor;
            yield return new WaitForSeconds(blinkTime);
        }
    }

    void SpawnStunCircle()
    {
        GameObject circle = Instantiate(
            stunCirclePrefab,
            transform.position,
            Quaternion.identity
        );

        ChristmasBell sc = circle.GetComponent<ChristmasBell>();
        sc.target = this.transform;
        sc.followDuration = stunCircleDuration; // 원 유지 시간
        sc.stunDuration = stunDuration;         // 적 스턴 시간
    }

}
