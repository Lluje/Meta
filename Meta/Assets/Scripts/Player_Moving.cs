using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class Player_Moving : MonoBehaviour
{
    [Header("이동 관련 설정")]
    public float moveSpeed = 3f;       // 자동 이동 속도
    public float jumpForce = 6f;       // 점프 힘
    private bool isGrounded = false;   // 땅에 닿아 있는지 여부

    private Rigidbody2D rb;
    private Animator animator;

    public bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AutoMove();
        Jump();
    }

    // 오른쪽으로 자동 이동
    void AutoMove()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    // 점프 입력 처리
    void Jump()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetMouseButtonDown(0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;

            if (animator != null)
                animator.SetTrigger("Jump");
        }
    }

    // 충돌 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground에 닿으면 착지
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        // Obstacle에 닿으면 게임오버
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        moveSpeed = 0; // 이동 정지

        if (animator != null)
        {
            animator.SetInteger("IsDie", 1);
            isDead = true;
        }

        Restart();
    }

    void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }
}