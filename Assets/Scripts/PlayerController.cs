using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;                // �̵� �ӵ�
    public float jumpForce = 10f;               // ���� ��
    public float coyoteTime = 0.2f;             // �ڿ��� Ÿ�� (���߿� �������� ���� ���� ������ �ð�)
    public float jumpBufferTime = 0.2f;         // ���� ���۸� �ð� (��ư �Է��� �޾Ƶ��̴� �ð�)
    public Transform groundCheck1;              // �ٴ� üũ ��ġ 1
    public Transform groundCheck2;              // �ٴ� üũ ��ġ 2
    public LayerMask groundLayer;               // �ٴ� ���̾� ����ũ
    public float gravityScale = 3f;             // �߷� ũ��
    public float lowJumpMultiplier = 2.5f;      // ���� ���� ���� ��Ƽ�ö��̾�
    public float fallMultiplier = 2.5f;         // ���� ���� ��Ƽ�ö��̾�
    public float dashSpeed = 20f;               // �뽬 �ӵ�
    public float dashTime = 0.2f;               // �뽬 ���� �ð�
    public float dashCooldown = 1f;             // �뽬 ��ٿ� �ð�
    public float umbrellaFallMultiplier = 0.5f; // ��� ���� �� �߷� ��Ƽ�ö��̾�
    public GameObject umbrella;                 // ��� ������Ʈ

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;                    // �ٴڿ� �ִ��� ����
    private float coyoteTimeCounter;            // �ڿ��� Ÿ�� ī����
    private float jumpBufferCounter;            // ���� ���۸� ī����
    private bool isJumping;                     // ���� ������ ����
    private bool isDashing;                     // �뽬 ������ ����
    private bool isUmbrellaOpen;                // ����� ���� �ִ��� ����
    private float dashCooldownCounter;          // �뽬 ��ٿ� ī����
    private float dashTimeCounter;              // �뽬 �ð� ī����
    private bool isFacingRight = true;          // �÷��̾ �������� ���� �ִ��� ����

    public bool isChargingJump;                // ��¡ ���� ������ ����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        // �뽬 ���� ���� �Է��� ����
        if (isDashing || isChargingJump)
        {
            return;
        }

        // �¿� �̵�
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ĳ���� ���� ����
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

        // �ٴ� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck1.position, 0.1f, groundLayer)
                   || Physics2D.OverlapCircle(groundCheck2.position, 0.1f, groundLayer);

        // �ڿ��� Ÿ��
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // ���� ���۸�
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // ����
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
        }

        // �߷� ����
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

        // �뽬
        if (Input.GetKeyDown(KeyCode.D) && dashCooldownCounter <= 0)
        {
            StartDash();
        }

        // �뽬 ��Ÿ�� ī����
        dashCooldownCounter -= Time.deltaTime;

        // ��� ����/�ݱ�
        if (Input.GetKeyDown(KeyCode.S))
        {
            isUmbrellaOpen = !isUmbrellaOpen;
            umbrella.SetActive(isUmbrellaOpen);
        }

        // ���� ���� (��¡)
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.A) && coyoteTimeCounter > 0)
        {
            StartCoroutine(ChargeJumpCoroutine());
        }
    }

    private IEnumerator ChargeJumpCoroutine()
    {
        isChargingJump = true; // ��¡ ���� ����

        float chargeTime = 0f;
        float maxChargeTime = 1f; // �ִ� ��¡ �ð� (���÷� 2�ʷ� ����)

        // ��¡ �ð� ���� Ű�� ������ ������ ���� ���� ����
        while (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.A) && chargeTime < maxChargeTime)
        {
            chargeTime += Time.deltaTime;
            yield return null;
        }

        // ��¡ �ð��� ������ ���� ����
        if (chargeTime > 0)
        {
            float jumpPower = jumpForce * (0.5f + chargeTime / maxChargeTime); // ��¡ �ð��� ���� ���� �� ����
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            coyoteTimeCounter = 0; // �ڿ��� Ÿ�� �ʱ�ȭ
        }

        isChargingJump = false; // ��¡ ���� ����
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeCounter = dashTime;
        dashCooldownCounter = dashCooldown;

        float dashDirection = isFacingRight ? 1 : -1; // ĳ������ ���⿡ ���� �뽬 ���� ����
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �𼭸� �浹 �� �뽬 �ڳ� ���� ���� �߰�
        if (isDashing)
        {
            // �뽬 �� �𼭸��� �ε����� �� ó�� ����
            ContactPoint2D[] contacts = new ContactPoint2D[10];
            int numContacts = collision.GetContacts(contacts);

            for (int i = 0; i < numContacts; i++)
            {
                if (contacts[i].normal.y < 0.5f && contacts[i].normal.y > -0.5f)
                {
                    // �𼭸��� �ε������Ƿ�, �÷��� ���� �ö󰡵��� ó��
                    Vector2 newPosition = rb.position + (contacts[i].normal * 0.1f);
                    rb.MovePosition(newPosition);
                    break;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // groundCheck ��ġ ǥ�� (������)
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

        // �ٴ� üũ ���� ǥ��
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck1.position, 0.1f);
        Gizmos.DrawWireSphere(groundCheck2.position, 0.1f);
    }
}
