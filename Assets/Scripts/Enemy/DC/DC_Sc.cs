using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float cooldownTime = 0.7f;
    public Vector2 detectionSize1 = new Vector2(5f, 2f); // 플레이어 감지 범위 (사각형 크기)
    public Vector2 detectionOffset1 = Vector2.zero; // 사각형의 위치 오프셋
    public LayerMask playerLayer1; // 플레이어 레이어
    public GameObject dashEffect;

    [Header("Detection info")]
    public Vector2 detectionSize = new Vector2(15f, 5f); // 플레이어 감지 범위 (사각형 크기)
    public Vector2 detectionOffset = Vector2.zero; // 사각형의 위치 오프셋
    public LayerMask playerLayer; // 플레이어 레이어
    public bool nmove = false;

    private Transform playerTransform;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기
        think();
    }

    void FixedUpdate()
    {
        if (move && !isDashing) // 대쉬 중이 아닐 때만 이동
        {
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);
        }
    }

    private void Update()
    {
        spriteRenderer.flipX = rigid.velocity.x > 0;
        DetectAndMoveTowardsPlayer();
        Dashcoll();
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

    void DetectAndMoveTowardsPlayer()
    {
        // 플레이어를 감지할 사각형 영역
        Collider2D[] player = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset, detectionSize, 0f, playerLayer);

        foreach (Collider2D collider in player)
        {
            if (collider.tag == "Player")
            {
                Debug.Log("플레이어 감지");
                playerTransform = collider.transform;
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                nextmove = directionToPlayer.x > 0 ? 1 : -1;
            }
        }
    }

    void Dashcoll()
    {
        // 플레이어를 감지할 사각형 영역
        Collider2D[] player1 = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset1, detectionSize1, 0f, playerLayer1);

        foreach (Collider2D collider in player1)
        {
            if (collider.tag == "Player" && !isPreparingToDash)
            {
                move = false;
                dashTarget = collider.transform.position; // 대쉬 목표 설정
                dashTarget.y = transform.position.y; // y축 자기걸로 변경
                StartCoroutine(PrepareAndDash(dashTarget));
            }
        }
    }



    private IEnumerator PrepareAndDash(Vector2 targetPosition)
    {
        isPreparingToDash = true; // 대쉬 준비 중으로 설정
        isDashing = true; // 대쉬 시작
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.7f); // 준비 시간 동안 대기

        dashTarget = targetPosition; // 대쉬 목표 설정

        GameObject explosion = Instantiate(dashEffect, this.transform.position, Quaternion.identity);
        Destroy(explosion, 0.5f);

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
        yield return new WaitForSeconds(cooldownTime); // 쿨타임 대기
        isDashing = false; // 대쉬 종료
        move = true;
        isPreparingToDash = false; // 대쉬 준비 완료
    }

    // 시각적으로 감지 범위 표시용 (디버그)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)detectionOffset, new Vector3(detectionSize.x, detectionSize.y, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + (Vector3)detectionOffset1, new Vector3(detectionSize1.x, detectionSize1.y, 0));
    }
}
