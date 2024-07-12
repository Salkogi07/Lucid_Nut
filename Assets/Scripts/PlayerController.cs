using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;                // 이동 속도
    public float jumpForce = 10f;               // 점프 힘
    public float coyoteTime = 0.2f;             // 코요테 타임 (공중에 떨어지기 전에 점프 가능한 시간)
    public float jumpBufferTime = 0.2f;         // 점프 버퍼링 시간 (버튼 입력을 받아들이는 시간)
    public Transform groundCheck1;              // 바닥 체크 위치 1
    public Transform groundCheck2;              // 바닥 체크 위치 2
    public LayerMask groundLayer;               // 바닥 레이어 마스크
    public float gravityScale = 3f;             // 중력 크기
    public float lowJumpMultiplier = 2.5f;      // 낮은 점프 감속 멀티플라이어
    public float fallMultiplier = 2.5f;         // 낙하 감속 멀티플라이어
    public float dashSpeed = 20f;               // 대쉬 속도
    public float dashTime = 0.2f;               // 대쉬 지속 시간
    public float dashCooldown = 1f;             // 대쉬 쿨다운 시간
    public float umbrellaFallMultiplier = 0.5f; // 우산 열림 중 중력 멀티플라이어
    public GameObject umbrella;                 // 우산 오브젝트

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;                    // 바닥에 있는지 여부
    private float coyoteTimeCounter;            // 코요테 타임 카운터
    private float jumpBufferCounter;            // 점프 버퍼링 카운터
    private bool isJumping;                     // 점프 중인지 여부
    private bool isDashing;                     // 대쉬 중인지 여부
    private bool isUmbrellaOpen;                // 우산이 열려 있는지 여부
    private float dashCooldownCounter;          // 대쉬 쿨다운 카운터
    private bool isFacingRight = false;          // 플레이어가 오른쪽을 보고 있는지 여부
    private int moveInput = 0;

    public bool isChargingJump;                 // 차징 점프 중인지 여부
    public Image chargeIndicator;               // UI Image 참조 변수
    public Image chargeIndicator_back;          // UI Image 참조 변수

    public float stopSmoothTime = 0.2f;         // 멈출 때의 감속 시간
    private float currentVelocityX;             // 현재 속도 (감속용)

    //private bool movecond = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        // 대쉬 중이거나 차징 중일 때는 이동 입력을 무시
        if (isDashing || isChargingJump)
        {
            return;
        }

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

        // 중력 조정
        GravitySetting();

        // 대쉬
        Dash();

        // 우산 열기/닫기
        Umbrella();

        // 슈퍼 점프 (차징)
        ChargeJump();
    }

    private void ChargeJump()
    {
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.A) && coyoteTimeCounter > 0)
        {
            StartCoroutine(ChargeJumpCoroutine());
        }

        chargeIndicator.fillAmount = 0f; // 처음에는 차징이 되지 않은 상태이므로 0으로 초기화
    }

    private void Umbrella()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            isUmbrellaOpen = !isUmbrellaOpen;
            umbrella.SetActive(isUmbrellaOpen);
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.D) && dashCooldownCounter <= 0)
        {
            StartDash();
        }

        // 대쉬 쿨타임 카운터
        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
        }
    }

    private void GravitySetting()
    {
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
        isGrounded = Physics2D.OverlapCircle(groundCheck1.position, 0.1f, groundLayer)
                           || Physics2D.OverlapCircle(groundCheck2.position, 0.1f, groundLayer);
    }

    private void CharacterFlip()
    {
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
    }

    void MoveInput()
    {
        moveInput = 0;

        //if (isChargingJump)
        //{
            // 차징 중일 때는 점진적으로 멈춤
           // Debug.Log("차지중일때 멈춤");
            //rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, 0, ref currentVelocityX, stopSmoothTime), rb.velocity.y);
        //}


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.A) && coyoteTimeCounter > 0)
            {
                Debug.Log("ds");
            }
            else
            {
                moveInput = -1;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.A) && coyoteTimeCounter > 0)
            {
                Debug.Log("ds");
            }
            else
            {
                moveInput = 1;
            }
        }
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private IEnumerator ChargeJumpCoroutine()
    {
        isChargingJump = true; // 차징 점프 시작
        chargeIndicator_back.gameObject.SetActive(true);

        float chargeTime = 0f;
        float maxChargeTime = 1f; // 최대 차징 시간 (예시로 2초로 설정)

        // 차징 시간 동안 키를 누르고 있으면 점프 힘을 축적
        while (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.A) && chargeTime < maxChargeTime)
        {
            chargeTime += Time.deltaTime;

            // UI의 fill amount 업데이트
            chargeIndicator.fillAmount = chargeTime / maxChargeTime;

            yield return null;
        }

        // 차징 시간이 끝나면 점프 실행
        if (chargeTime > 0)
        {
            float jumpPower = jumpForce * (0.5f + chargeTime / maxChargeTime); // 차징 시간에 따라 점프 힘 조정
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            coyoteTimeCounter = 0; // 코요테 타임 초기화
        }

        isChargingJump = false; // 차징 점프 종료

        // UI fill amount 초기화
        chargeIndicator.fillAmount = 0f;
        chargeIndicator_back.gameObject.SetActive(false);
    }

    private void StartDash()
    {
        isDashing = true;
        dashCooldownCounter = dashCooldown;

        // 플레이어가 바라보는 방향에 따라 대쉬 방향 설정
        float dashDirection = isFacingRight ? 1 : -1;

        // 플레이어의 현재 속도 방향과 상관없이 대쉬 속도로 설정
        rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(rb.velocity.x, 0);

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    /*void OnCollisionEnter2D(Collision2D collision)
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
    }*/

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
