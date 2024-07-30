using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum eState
    {
        IDLE, MOVE, ATTACK, MOVINGATTACK, GETHIT, DIE, JUMP
    }
    private eState state = eState.IDLE;
    private bool isDie;
    private Vector2 moveDir;
    private Animator anim;
    private Coroutine stateRoutine;
    private Coroutine actionRoutine;
    private bool isMovingAttack;
    private float holdingTime;
    private bool isJumping;

    public float jumpForce = 10f;
    private Rigidbody2D rBody2D;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rBody2D = GetComponent<Rigidbody2D>();

        if (stateRoutine != null)
        {
            StopCoroutine(stateRoutine);
        }
        stateRoutine = StartCoroutine(CheckState());

        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
        }
        actionRoutine = StartCoroutine(PlayerAction());
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.X))
        {
            OnAttack();
        }

        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(HandleJump());
        }

        if (Input.GetButtonUp("Jump"))
        {
            Jump();
        }
    }

    private void OnAttack()
    {
        state = eState.ATTACK;
        if (moveDir != Vector2.zero)
        {
            isMovingAttack = true;
        }
    }

    private IEnumerator HandleJump()
    {
        float holdStartTime = Time.time;
        while (Input.GetButton("Jump"))
        {
            holdingTime = Time.time - holdStartTime;
            yield return null;
        }
    }

    private void Jump()
    {
        if (holdingTime > 0.5f && holdingTime < 1.5f)
        {
            // 보통 점프
        }
        else if (holdingTime > 1.5f)
        {
            Debug.Log("최대 유지 시간 초과");
            holdingTime = 1.5f;
        }
        else
        {
            holdingTime = 0.8f;
        }

        rBody2D.AddForce(Vector2.up * jumpForce * holdingTime, ForceMode2D.Impulse);
        state = eState.JUMP;
        holdingTime = 0;
    }

    private IEnumerator PlayerAction()
    {
        while (!isDie)
        {
            yield return null;
            if (state == eState.MOVE)
            {
                anim.SetInteger("State", 1);
            }
            else if (state == eState.ATTACK)
            {
                anim.SetInteger("State", 2);
            }
            else if (state == eState.MOVINGATTACK)
            {
                anim.SetInteger("State", 2);
                yield return new WaitForSeconds(0.23f);
                state = eState.MOVE;
            }
            else if (state == eState.JUMP)
            {
                anim.SetInteger("State", 3);
            }
            else
            {
                anim.SetInteger("State", 0);
            }
        }
    }

    private IEnumerator CheckState()
    {
        while (!isDie)
        {
            isMovingAttack = false;
            yield return null;
            if (moveDir != Vector2.zero && moveDir.y == 0)
            {
                transform.localScale = new Vector2(Mathf.Sign(moveDir.x), 1);
                state = eState.MOVE;
                transform.Translate(Vector2.right * moveDir.x * 4.0f * Time.deltaTime);

                if (isMovingAttack)
                {
                    state = eState.MOVINGATTACK;
                }
            }
            else
            {
                state = eState.IDLE;
            }
        }
    }
}
