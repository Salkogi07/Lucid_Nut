using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public Transform groundCheck1;
    public Transform groundCheck2;
    public LayerMask groundLayer;
    public float gravityScale = 3f;
    public float lowJumpMultiplier = 2.5f;
    public float fallMultiplier = 2.5f;
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    public float umbrellaFallMultiplier = 0.5f;
    public GameObject umbrella;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;
    private bool isDashing;
    private bool isUmbrellaOpen;
    private float dashCooldownCounter;
    private float dashTimeCounter;
    private bool isFacingRight = true; // 플레이어가 오른쪽을 보고 있는지 여부를 나타내는 변수

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        if (isDashing)
        {
            return; // 대쉬 중일 때는 다른 입력을 무시
        }

        // 좌우 이동
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // 캐릭터 방향 설정
        if (moveInput < 0)
        {
            isFacingRight = false;
            spriteRenderer.flipX = false;
        }
        else if (moveInput > 0)
        {
            isFacingRight = true;
            spriteRenderer.flipX = true;
        }

        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck1.position, 0.1f, groundLayer)
                   || Physics2D.OverlapCircle(groundCheck2.position, 0.1f, groundLayer);

        // 코요테 타임
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // 점프 버퍼링
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // 점프
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
        }

        // 중력 조정
        if (isUmbrellaOpen && rb.velocity.y <= 0)
        {
            rb.gravityScale = umbrellaFallMultiplier;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }

        // 대쉬
        if (Input.GetKeyDown(KeyCode.D) && dashCooldownCounter <= 0)
        {
            StartDash();
        }

        // 대쉬 쿨타임 카운터
        dashCooldownCounter -= Time.deltaTime;

        // 우산 열기/닫기
        if (Input.GetKeyDown(KeyCode.S))
        {
            isUmbrellaOpen = !isUmbrellaOpen;
            umbrella.SetActive(isUmbrellaOpen);
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeCounter = dashTime;
        dashCooldownCounter = dashCooldown;

        float dashDirection = isFacingRight ? 1 : -1; // 캐릭터의 방향에 따라 대쉬 방향 설정
        rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);

        StartCoroutine(DashCoroutine());
    }

    private System.Collections.IEnumerator DashCoroutine()
    {
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(rb.velocity.x, 0);

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 모서리 충돌 및 대쉬 코너 조정 로직 추가
        if (isDashing)
        {
            // 대쉬 중 모서리에 부딪혔을 때 처리 예시
            ContactPoint2D[] contacts = new ContactPoint2D[10];
            int numContacts = collision.GetContacts(contacts);

            for (int i = 0; i < numContacts; i++)
            {
                if (contacts[i].normal.y < 0.5f && contacts[i].normal.y > -0.5f)
                {
                    // 모서리에 부딪혔으므로, 플랫폼 위로 올라가도록 처리
                    Vector2 newPosition = rb.position + (contacts[i].normal * 0.1f);
                    rb.MovePosition(newPosition);
                    break;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // groundCheck 위치 표시 (디버깅용)
        if (groundCheck1 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck1.position, 0.1f);
        }

        if (groundCheck2 != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheck2.position, 0.1f);
        }

        // 바닥 체크 범위 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck1.position, 0.1f);
        Gizmos.DrawWireSphere(groundCheck2.position, 0.1f);
    }
}
