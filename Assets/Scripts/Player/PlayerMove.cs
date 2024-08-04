using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] public float moveSpeed = 5f;                // �̵� �ӵ�
    [SerializeField] public float jumpForce = 10f;               // ���� ��

    [SerializeField] public float coyoteTime = 0.2f;
    [SerializeField] public float jumpBufferTime = 0.2f;

    [Header("Ground Check")]
    [SerializeField] private bool isGrounded;                    // �ٴڿ� �ִ��� ����
    [SerializeField] public float groundChekDistance;
    [SerializeField] public Transform groundCheck1;              // �ٴ� üũ ��ġ 1
    [SerializeField] public Transform groundCheck2;              // �ٴ� üũ ��ġ 2
    [SerializeField] public LayerMask groundLayer;               // �ٴ� ���̾� ����ũ


    [Header("Component")]
    public Rigidbody2D rb { get; private set; }
    private PlayerAnimator animator;

    [Header("IsAtcitoning")]
    [SerializeField] public bool isPlatform = false;
    [SerializeField] private bool isJumping;                     // ���� ������ ����
    [SerializeField] private bool isFacingRight = false;         // �÷��̾ �������� ���� �ִ��� ����

    private int facingDir;
    private int moveInput = 0;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    public bool isDashing;
    public bool canDash = true;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        Jump();

        AnimationController();
    }


    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        // �¿� �̵�
        MoveInput();

        // ĳ���� ���� ����
        Flip();

        // �ٴ� üũ
        GroundCheck();
    }
    
    void MoveInput()
    {
        moveInput = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1;
        }
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    private void Jump()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
    }
    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
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



    void OnDrawGizmos()
    {
        // �ٴ� üũ ���� ǥ��
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheck1.position, new Vector3(groundCheck1.position.x, groundCheck1.position.y - groundChekDistance));
        Gizmos.DrawLine(groundCheck2.position, new Vector3(groundCheck2.position.x, groundCheck2.position.y - groundChekDistance));
    }
}
