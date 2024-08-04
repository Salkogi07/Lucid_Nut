using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;                // �̵� �ӵ�
    public float jumpForce = 10f;               // ���� ��
    public float coyoteTime = 0.2f;             // �ڿ��� Ÿ�� (���߿� �������� ���� ���� ������ �ð�)
    public float jumpBufferTime = 0.2f;         // ���� ���۸� �ð� (��ư �Է��� �޾Ƶ��̴� �ð�)
    public float groundChekDistance;
    public bool isPlatform = false;
    public Transform groundCheck1;              // �ٴ� üũ ��ġ 1
    public Transform groundCheck2;              // �ٴ� üũ ��ġ 2
    public LayerMask groundLayer;               // �ٴ� ���̾� ����ũ
    public LayerMask platformLayer;             // �÷��� ���̾� ����ũ

    public Rigidbody2D rb { get; private set; }
    private PlayerAnimator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;                    // �ٴڿ� �ִ��� ����
    private float coyoteTimeCounter;            // �ڿ��� Ÿ�� ī����
    private float jumpBufferCounter;            // ���� ���۸� ī����
    private bool isJumping;                     // ���� ������ ����
    //private bool isFacingRight = false;         // �÷��̾ �������� ���� �ִ��� ����
    private int moveInput = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<PlayerAnimator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // �¿� �̵�
        MoveInput();

        // ĳ���� ���� ����
        CharacterFlip();

        // �ٴ� üũ
        GroundCheck();

        // �ڿ��� Ÿ��
        CoyoteTime();

        // ���� ���۸�
        JumpBuffering();

        // ����
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
            isJumping = true; // ���� ������ ǥ��
        }

        // �ٴڿ� ������ ���� ���¸� ����
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
        // �ٴ� üũ ���� ǥ��
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheck1.position, new Vector3(groundCheck1.position.x, groundCheck1.position.y - groundChekDistance));
        Gizmos.DrawLine(groundCheck2.position, new Vector3(groundCheck2.position.x, groundCheck2.position.y - groundChekDistance));
    }
}
