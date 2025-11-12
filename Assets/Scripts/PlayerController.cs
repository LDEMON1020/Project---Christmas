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
    
    }
    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        //캐릭터 좌우 반전
        if (moveInput > 0)
            sr.flipX = false; // 오른쪽


        else if (moveInput < 0)
            sr.flipX = true; // 왼쪽
  
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

   

    // 땅 체크 시각화 (Scene에서 보임)
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        hpSlider.value = (float)currentHP / maxHP;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void EnableCandyAttackCollider()
    {
        candyAttack.EnableWeaponCollider();
    }

    public void DisableCandyAttackCollider()
    {
        candyAttack.DisableWeaponCollider();
    }

}
