using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DC_Sc : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer; // SpriteRenderer 추가

    [Header("Move info")]
    private Rigidbody2D rigid;
    public bool move = true;
    public float moveSpeed = 3f;
    public int nextmove;

    [Header("Dash info")]
    public bool isDashing = false; // 대쉬 중 여부
    public bool isPreparingToDash = false; // 대쉬 준비 중 여부
    private Vector2 dashTarget;
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float cooldownTime = 1f;

    [Header("Detection info")]
    public float detectionRange = 5f; // 플레이어 감지 범위
    public LayerMask playerLayer; // 플레이어 레이어
    private bool nmove = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기
        think();
    }

    void FixedUpdate()
    {
        // 플레이어 감지 및 방향 설정
        DetectPlayer();

        if (move && !isDashing) // 대쉬 중이 아닐 때만 이동
        {
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);
            // 이동 방향에 따른 스프라이트 방향 조정
            if (rigid.velocity.x != 0)
            {
                spriteRenderer.flipX = rigid.velocity.x < 0;
            }
        }
        else if (isDashing)
        {
            // 대쉬 중에도 스프라이트 방향 조정
            Vector2 direction = (dashTarget - (Vector2)transform.position).normalized;
            spriteRenderer.flipX = direction.x > 0;
        }
    }

    void DetectPlayer()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D rayHitLeft = Physics2D.Raycast(transform.position, Vector2.left, detectionRange, playerLayer);
        RaycastHit2D rayHitRight = Physics2D.Raycast(transform.position, Vector2.right, detectionRange, playerLayer);
        if (rayHitLeft.collider != null && rayHitLeft.collider.CompareTag("Player"))
        {
            Debug.Log("어쩔");
            nmove = true;
            nextmove = -1; // 왼쪽으로 이동
        }
        else if (rayHitRight.collider != null && rayHitRight.collider.CompareTag("Player"))
        {
            nmove = true;
            nextmove = 1; // 오른쪽으로 이동
        }
        else
        {
            nmove = false;
        }

        // 스프라이트 방향 조정
        if (nextmove != 0)
        {
            spriteRenderer.flipX = nextmove < 0;
        }
    }

    void think()
    {
        if (!nmove)
        {
            nextmove = Random.Range(-1, 2);
        }
        float nextTimeThink = Random.Range(2f, 4f);
        Invoke("think", nextTimeThink);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !isPreparingToDash)
        {
            move = false;
            dashTarget = other.transform.position; // 대쉬 목표 설정
            dashTarget.y = transform.position.y; // y축 자기걸로 변경

            // 대쉬 방향 판별 및 스프라이트 방향 조정
            Vector2 direction = (dashTarget - (Vector2)transform.position).normalized;
            spriteRenderer.flipX = direction.x < 0;

            // 대쉬 시작
            animator.SetBool("Attack", true);
            StartCoroutine(PrepareAndDash(dashTarget));
        }
        if (other.CompareTag("wall"))
        {
            nextmove *= -1;
            CancelInvoke();
        }
    }

    private IEnumerator PrepareAndDash(Vector2 targetPosition)
    {
        isPreparingToDash = true; // 대쉬 준비 중으로 설정
        yield return new WaitForSeconds(0.7f); // 준비 시간 동안 대기

        dashTarget = targetPosition; // 대쉬 목표 설정
        isPreparingToDash = false; // 대쉬 준비 완료
        isDashing = true; // 대쉬 시작

        // 대쉬 중 목표 위치로 이동
        while (Vector2.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null; // 프레임마다 대기

            // 대쉬 중에도 스프라이트 방향 조정
            Vector2 direction = (dashTarget - (Vector2)transform.position).normalized;
            spriteRenderer.flipX = direction.x > 0;
        }

        // 대쉬 완료 후 방향 조정
        animator.SetBool("Attack", false);
        isDashing = false; // 대쉬 종료
        yield return new WaitForSeconds(cooldownTime); // 쿨타임 대기
        move = true;
    }
}
