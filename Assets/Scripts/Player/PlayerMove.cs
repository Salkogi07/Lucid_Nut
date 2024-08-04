using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;                // 이동 속도
    public float jumpForce = 10f;               // 점프 힘
    public float coyoteTime = 0.2f;             // 코요테 타임 (공중에 떨어지기 전에 점프 가능한 시간)
    public float jumpBufferTime = 0.2f;         // 점프 버퍼링 시간 (버튼 입력을 받아들이는 시간)
    public float groundChekDistance;
    public bool isPlatform = false;
    public Transform groundCheck1;              // 바닥 체크 위치 1
    public Transform groundCheck2;              // 바닥 체크 위치 2
    public LayerMask groundLayer;               // 바닥 레이어 마스크
    public LayerMask platformLayer;             // 플랫폼 레이어 마스크

    public Rigidbody2D rb { get; private set; }
    private PlayerAnimator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;                    // 바닥에 있는지 여부
    private float coyoteTimeCounter;            // 코요테 타임 카운터
    private float jumpBufferCounter;            // 점프 버퍼링 카운터
    private bool isJumping;                     // 점프 중인지 여부
    //private bool isFacingRight = false;         // 플레이어가 오른쪽을 보고 있는지 여부
    private int moveInput = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<PlayerAnimator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // 좌우 이동
        MoveInput();

        // 캐릭터 방향 설정
        CharacterFlip();

        // 바닥 체크
        GroundCheck();

        // 코요테 타임
        CoyoteTime();

        // 점프 버퍼링
        JumpBuffering();

        // 점프
        Jump();

        AnimationController();
    }

    private void AnimationController()
    {
        if (!isGrounded && rb.velocity.y < 0)
        {
            animator.PlayAnimation("Fall");
        }
        else if (!isGrounded && rb.velocity.y > 0)
        {
            animator.PlayAnimation("Jump");
        }
        else if (isGrounded && moveInput != 0)
        {
            animator.PlayAnimation("Move");
        }
        else if (isGrounded && moveInput == 0)
        {
            animator.PlayAnimation("Idle");
        }
    }

    private void Jump()
    {
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
            isJumping = true; // 점프 중임을 표시
        }

        // 바닥에 닿으면 점프 상태를 리셋
        if (isGrounded && rb.velocity.y <= 0)
        {
            isJumping = false;
        }
    }

    private void JumpBuffering()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void CoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void GroundCheck()
    {
        if (!isPlatform)
        {
            isGrounded = Physics2D.Raycast(groundCheck1.position, Vector2.down, groundChekDistance, groundLayer)
                               || Physics2D.Raycast(groundCheck2.position, Vector2.down, groundChekDistance, groundLayer);
        }
        else
        {
            isGrounded = false;
        }
    }

    private void CharacterFlip()
    {
        if (moveInput < 0)
        {
            //isFacingRight = false;
            spriteRenderer.flipX = false;
        }
        else if (moveInput > 0)
        {
            //isFacingRight = true;
            spriteRenderer.flipX = true;
        }
    }

    void MoveInput()
    {
        moveInput = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1;
        }
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void OnDrawGizmos()
    {
        // 바닥 체크 범위 표시
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheck1.position, new Vector3(groundCheck1.position.x, groundCheck1.position.y - groundChekDistance));
        Gizmos.DrawLine(groundCheck2.position, new Vector3(groundCheck2.position.x, groundCheck2.position.y - groundChekDistance));
    }
}
