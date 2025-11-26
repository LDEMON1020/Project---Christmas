using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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


    private Rigidbody2D rb;
    public SpriteRenderer candySr;
    public SpriteRenderer sr;
  
    [Header("땅 체크")]
    public Transform groundCheck;           //땅 체크용
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;           //ground 레이어 설정 필요

    public CandyAttack candyAttack;

    public GameObject gameOverPanel;        //게임 오버 판넬

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

    void Die()
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



}
