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
    private bool isFacingRight = true; // �÷��̾ �������� ���� �ִ��� ���θ� ��Ÿ���� ����

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
            return; // �뽬 ���� ���� �ٸ� �Է��� ����
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
