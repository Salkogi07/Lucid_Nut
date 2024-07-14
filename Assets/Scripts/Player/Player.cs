using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float gravityScale = 3.5f;
    //public float lowJumpMultiplier = 2.5f;      // 낮은 점프 감속 멀티플라이어
    //public float fallMultiplier = 2.5f;         // 낙하 감속 멀티플라이어

    [Header("Umbrella info")]
    public bool isUmbrellaOpen = false;
    public float umbrellaFallMultiplier = 0.5f; // 우산 열림 중 중력 멀티플라이어
    public GameObject umbrellaObj;                 // 우산 오브젝트

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    [Header("ChargeJump info")]
    public bool isChargeJump;
    public Image chargeIndicator;               // UI Image 참조 변수
    public Image chargeIndicator_back;          // UI Image 참조 변수

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundChekDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    #region Componets
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

        CheckDash_Input();
        CheckChargeJump_Input();
        CheckUmbrella_Input();
        Debug.Log(stateMachine.currentState.ToString());
    }

    private void CheckUmbrella_Input()
    {
        if (Input.GetKeyDown(KeyCode.S))
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

    private void CheckChargeJump_Input()
    {
        if (isUmbrellaOpen && IsGroundDetected())
        {
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.A))
            {
                stateMachine.ChangeState(chargeJump);
            }

            chargeIndicator.fillAmount = 0f; // 처음에는 차징이 되지 않은 상태이므로 0으로 초기화
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
}
