using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float inputThreshold = 0.1f;
    public float gravityScale = 3.5f;
    public float jumpGravityScale = 1.75f;
    public float fallMultiplier = 2.5f;

    [Header("Umbrella info")]
    public bool isUmbrellaOpen = false;
    public float umbrellaFallMultiplier = 0.5f;
    public GameObject umbrellaObj;

    [Header("Dash info")]
    [SerializeField] public float dashCooldown;
    public bool isDashing = true;
    public float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir;

    [Header("ChargeJump info")]
    public bool isChargeJump_inputKey = false;
    public bool isChargeJump;
    public Image chargeIndicator;
    public Image chargeIndicator_back;

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundChekDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    [Header("Jump Timing Info")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerChargeJump chargeJump { get; private set; }
    #endregion

    public bool isJumping = false;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        chargeJump = new PlayerChargeJump(this, stateMachine, "ChargeJump");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = gravityScale;

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();

        if (!(stateMachine.currentState is PlayerChargeJump))
        {
            CheckDash_Input();
            CheckUmbrella_Input();
            UpdateCoyoteTime();
            UpdateJumpBuffer();

            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !isJumping && !isChargeJump)
            {
                jumpBufferCounter = 0;
                coyoteTimeCounter = 0;
                stateMachine.ChangeState(jumpState);
            }
        }
    }

    private void UpdateCoyoteTime()
    {
        if (IsGroundDetected())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    private void UpdateJumpBuffer()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isChargeJump && !Input.GetKey(KeyCode.UpArrow))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;
    }

    private void CheckUmbrella_Input()
    {
        if (Input.GetKeyDown(KeyCode.S) && !isChargeJump)
        {
            isUmbrellaOpen = !isUmbrellaOpen;
            umbrellaObj.SetActive(isUmbrellaOpen);
        }
    }

    private void CheckDash_Input()
    {
        if (!isChargeJump)
        {
            dashUsageTimer -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.D) && dashUsageTimer < 0)
            {
                dashUsageTimer = dashCooldown;
                dashDir = Input.GetAxisRaw("Horizontal");

                if (dashDir == 0)
                    dashDir = facingDir;

                stateMachine.ChangeState(dashState);
            }
        }
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundChekDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundChekDistance));
    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    public void StartPlayerCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public bool CanJump()
    {
        return coyoteTimeCounter > 0 || IsGroundDetected();
    }

    public void ResetJumpBuffer()
    {
        jumpBufferCounter = 0;
    }

    public void ResetCoyoteTime()
    {
        coyoteTimeCounter = 0;
    }
}
