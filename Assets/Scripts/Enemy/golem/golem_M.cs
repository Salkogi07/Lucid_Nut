using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class golem_M : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer; // SpriteRenderer 추가

    [Header("Move info")]
    private Rigidbody2D rigid;
    public bool move = true;
    public float moveSpeed = 3f;
    public int nextmove;

    [Header("Attack info")]
    public Vector2 detectionSize1 = new Vector2(10f, 2f); // 플레이어 감지 범위 (사각형 크기)
    public Vector2 detectionOffset1 = Vector2.zero; // 사각형의 위치 오프셋
    public LayerMask playerLayer1; // 플레이어 레이어
    public bool attackC = true;

    [Header("Detection info")]
    public Vector2 detectionSize = new Vector2(22f, 2f); // 플레이어 감지 범위 (사각형 크기)
    public Vector2 detectionOffset = Vector2.zero; // 사각형의 위치 오프셋
    public LayerMask playerLayer; // 플레이어 레이어
    public bool nmove = false;

    private Transform playerTransform;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기
        think();
    }

    private void Update()
    {
        Attack();
        DetectAndMoveTowardsPlayer();
        if (move)
        {
            moveSpeed = 3f;
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);
            spriteRenderer.flipX = rigid.velocity.x > 0;
        }
    }

    void think()
    {
        if (nmove)
        {
            nextmove = Random.Range(-1, 2);
            if (nextmove == 0)
            {
                animator.SetBool("move", false);
            }
            else
            {
                animator.SetBool("move", true);
            }
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
            if (collider.tag == "Player" && attackC)
            {
                playerTransform = collider.transform;
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                nextmove = directionToPlayer.x > 0 ? 1 : -1;
            }
        }
    }

    void Attack()
    {
        // 플레이어를 감지할 사각형 영역
        Collider2D[] players = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset1, detectionSize1, 0f, playerLayer1);

        foreach (Collider2D collider in players)
        {
            if (collider.tag == "Player" && attackC)
            {
                animator.SetBool("move", true);
                attackC = false;
                StartCoroutine(FireProjectile(collider.transform.position));
                break; // 감지된 플레이어가 있으면 한 번만 발사
            }
        }
    }

    private IEnumerator FireProjectile(Vector2 targetPosition)
    {
        Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;
        nextmove = directionToPlayer.x > 0 ? 1 : -1;
        spriteRenderer.flipX = nextmove > 0;

        // 스킬 사용 중 이동을 멈추고 애니메이션 시작
        nextmove = 0;
        nmove = false;
        move = false;
        moveSpeed = 0f;
        animator.SetBool("Attack", true);
        animator.SetBool("move", false);
        yield return new WaitForSeconds(1f);


        animator.SetBool("Attack", false);
        animator.SetBool("move", true);
        yield return new WaitForSeconds(1f);
        nmove = true;
        nextmove = Random.Range(-1, 2);
        move = true;
        attackC = true;
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
