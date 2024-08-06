using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] public float moveSpeed = 5f; // 이동 속도
    [SerializeField] public float jumpForce = 10f; // 점프 힘

    [SerializeField] public float coyoteTime = 0.2f;
    [SerializeField] public float jumpBufferTime = 0.2f;

    private float gravityScale = 3.5f;

    [Header("Ground Check")]
    [SerializeField] private bool isGrounded; // 바닥에 있는지 여부
    [SerializeField] public float groundCheckDistance;
    [SerializeField] public Transform groundCheck1; // 바닥 체크 위치 1
    [SerializeField] public Transform groundCheck2; // 바닥 체크 위치 2
    [SerializeField] public LayerMask groundLayer; // 바닥 레이어 마스크

    [Header("Component")]
    public Rigidbody2D rb { get; private set; }
    private PlayerAnimator animator;
    private PlayerSkill playerSkill;

    [Header("IsAtcitoning")]
    [SerializeField] public bool isPlatform = false;
    [SerializeField] private bool isJumping; // 점프 중인지 여부
    [SerializeField] private bool isFacingRight = false; // 플레이어가 오른쪽을 보고 있는지 여부

    private int facingDir;
    private int moveInput = 0;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    public bool isDashing = false;
    public bool isAttack = false;

    // 추가된 변수들
    [Header("Double Jump")]
    [SerializeField] private bool canDoubleJump = true; // 더블 점프 기능을 켜고 끄는 변수
    private bool doubleJumpAvailable = false; // 더블 점프 가능 여부

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSkill = GetComponent<PlayerSkill>();
        animator = GetComponent<PlayerAnimator>();

        gravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        Jump();

        GravitySetting();

        AnimationController();
    }

    private void GravitySetting()
    {
        if (playerSkill.isUmbrellaOpen && rb.velocity.y <= 0)
        {
            rb.gravityScale = playerSkill.umbrellaFallMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        // 좌우 이동
        MoveInput();

        // 캐릭터 방향 설정
        Flip();

        // 바닥 체크
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
            doubleJumpAvailable = true; // 바닥에 닿으면 더블 점프 가능
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
            PerformJump();
        }
        else if (canDoubleJump && doubleJumpAvailable && !isGrounded && Input.GetButtonDown("Jump"))
        {
            PerformJump();
            doubleJumpAvailable = false; // 더블 점프 사용 후에는 더블 점프 불가
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    }

    private void PerformJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpBufferCounter = 0f;
        StartCoroutine(JumpCooldown());
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    public bool canUmbrella()
    {
        return !isDashing && !isAttack;
    }

    private void GroundCheck()
    {
        if (!isPlatform)
        {
            isGrounded = Physics2D.Raycast(groundCheck1.position, Vector2.down, groundCheckDistance, groundLayer)
                        || Physics2D.Raycast(groundCheck2.position, Vector2.down, groundCheckDistance, groundLayer);
        }
        else
        {
            isGrounded = false;
        }
    }

    private void AnimationController()
    {
        if (isAttack)
        {
            animator.PlayAnimation("Attack");
        }
        else if (!isGrounded && rb.velocity.y < 0)
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
        // 바닥 체크 범위 표시
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheck1.position, new Vector3(groundCheck1.position.x, groundCheck1.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundCheck2.position, new Vector3(groundCheck2.position.x, groundCheck2.position.y - groundCheckDistance));
    }
}
