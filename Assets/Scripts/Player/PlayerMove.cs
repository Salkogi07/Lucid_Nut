using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    #region MO
    [Header("MO")]
    public bool isMO = false;
    public Button leftButton;
    public Button rightButton;
    #endregion

    [Header("Player Info")]
    [SerializeField] public float moveSpeed = 5f; // �̵� �ӵ�
    [SerializeField] public float jumpForce = 10f; // ���� ��

    [SerializeField] public float coyoteTime = 0.2f;
    [SerializeField] public float jumpBufferTime = 0.2f;

    private float gravityScale = 3.5f;

    [Header("Ground Check")]
    [SerializeField] private bool isGrounded; // �ٴڿ� �ִ��� ����
    [SerializeField] public float groundCheckDistance;
    [SerializeField] public Transform groundCheck; // �ٴ� üũ ��ġ
    [SerializeField] public Vector2 groundCheckSize = new Vector2(1f, 0.1f); // �ٴ� üũ �ڽ� ũ��
    [SerializeField] public LayerMask groundLayer; // �ٴ� ���̾� ����ũ

    [Header("Component")]
    public GameObject wingBong;
    public ParticleSystem dust;
    public Rigidbody2D rb { get; private set; }
    private PlayerAnimator_M animator;
    private PlayerSkill playerSkill;

    [Header("IsAtcitoning")]
    [SerializeField] public bool isPlatform = false;
    [SerializeField] private bool isJumping; // ���� ������ ����
    [SerializeField] private bool isFacingRight = false; // �÷��̾ �������� ���� �ִ��� ����

    private int facingDir;
    private int moveInput = 0;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    public bool isDashing = false;
    public bool isAttack = false;
    private bool isJumpCut = false;

    // �߰��� ������
    [Header("Double Jump")]
    [SerializeField] private bool canDoubleJump = true; // ���� ���� ����� �Ѱ� ���� ����
    private bool doubleJumpAvailable = false; // ���� ���� ���� ����

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSkill = GetComponent<PlayerSkill>();
        animator = GetComponent<PlayerAnimator_M>();

        gravityScale = rb.gravityScale;
    }

    #region MO
    private void Start()
    {
        AddEventTrigger(leftButton.gameObject, EventTriggerType.PointerDown, () => MoveLeft());
        AddEventTrigger(leftButton.gameObject, EventTriggerType.PointerUp, () => StopMovement());

        AddEventTrigger(rightButton.gameObject, EventTriggerType.PointerDown, () => MoveRight());
        AddEventTrigger(rightButton.gameObject, EventTriggerType.PointerUp, () => StopMovement());
    }
    #endregion

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (isMO)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            JumpMO();
        }
        else
        {
            // �¿� �̵�
            MoveInput();
            Jump();
        }

        // ĳ���� ���� ����
        Flip();

        // �ٴ� üũ
        GroundCheck();


        GravitySetting();

        AnimationController();
    }

    #region MO
    private void AddEventTrigger(GameObject obj, EventTriggerType type, UnityEngine.Events.UnityAction action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null) trigger = obj.AddComponent<EventTrigger>();

        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((eventData) => { action(); });
        trigger.triggers.Add(entry);
    }

    public void MoveRight()
    {
        moveInput = 1;
    }

    public void MoveLeft()
    {
        moveInput = -1;
    }

    public void StopMovement()
    {
        moveInput = 0;
    }

    public void OnJumpButtonPressed()
    {
        jumpBufferCounter = jumpBufferTime;
    }

    private void JumpMO()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            doubleJumpAvailable = true; // �ٴڿ� ������ ���� ���� ����
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // ���� ���� ó�� ����
        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            CreateDust();
            PerformJump();
            isJumpCut = true;
        }
        else if (canDoubleJump && doubleJumpAvailable && !isGrounded && jumpBufferCounter > 0f)
        {
            PerformJump();
            StartCoroutine(WingEffectStart());
            isJumpCut = true;
            doubleJumpAvailable = false; // ���� ���� ��� �Ŀ��� ���� ���� �Ұ�
        }

        if (isJumpCut && jumpBufferCounter <= 0f && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
            isJumpCut = false;
        }
    }
    #endregion

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
            if(isGrounded)
                CreateDust();

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
            doubleJumpAvailable = true; // �ٴڿ� ������ ���� ���� ����
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
            if(jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.deltaTime;
            }
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            CreateDust();
            PerformJump();
            isJumpCut = true;
        }
        else if (canDoubleJump && doubleJumpAvailable && !isGrounded && Input.GetButtonDown("Jump"))
        {
            PerformJump();
            StartCoroutine(WingEffectStart());
            isJumpCut = true;
            doubleJumpAvailable = false; // ���� ���� ��� �Ŀ��� ���� ���� �Ұ�
        }

        if (isJumpCut && Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
            isJumpCut = false;
        }
    }

    IEnumerator WingEffectStart()
    {
        wingBong.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        wingBong.SetActive(false);
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
            Collider2D collider = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
            isGrounded = collider != null;
        }
        else
        {
            isGrounded = false;
        }
    }

    void CreateDust()
    {
        dust.Play();
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
        // �ٴ� üũ ���� ǥ��
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
