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
    public int damage;
    public GameObject explosionEffect;

    [Header("Detection info")]
    public Vector2 detectionSize = new Vector2(22f, 2f); // 플레이어 감지 범위 (사각형 크기)
    public Vector2 detectionOffset = Vector2.zero; // 사각형의 위치 오프셋
    public LayerMask playerLayer; // 플레이어 레이어
    public bool nmove = true;

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
            if (nextmove == 0)
            {
                animator.SetBool("move", false);
            }
            else
            {
                animator.SetBool("move", true);
            }
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
            Debug.Log(nextmove);
        }
        float nextTimeThink = Random.Range(2f, 4f);
        Invoke("think", nextTimeThink);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            nextmove *= -1;
        }
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
                animator.SetBool("move", true);
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
                PlayerHp player = collider.GetComponent<PlayerHp>();
                StartCoroutine(FireProjectile(collider.transform.position, player, collider));
            }
        }
    }

    private IEnumerator FireProjectile(Vector2 targetPosition, PlayerHp player, Collider2D coll)
    {
        Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;
        nextmove = directionToPlayer.x > 0 ? 1 : -1;
        spriteRenderer.flipX = nextmove > 0;

        Vector2 expos = targetPosition;
        expos.x += -.15f * nextmove;

        // 스킬 사용 중 이동을 멈추고 애니메이션 시작
        nextmove = 0;
        nmove = false;
        move = false;
        moveSpeed = 0f;
        attackC = false;
        animator.SetBool("move", false);
        GameObject explosion = Instantiate(explosionEffect, expos, Quaternion.identity);
        Destroy(explosion, 2.5f);
        yield return new WaitForSeconds(1.1f);
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.8f);

        Collider2D[] playersInAttackRange = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset1, detectionSize1, 0f, playerLayer1);
        bool isPlayerInRange = false;
        foreach (Collider2D col in playersInAttackRange)
        {
            if (col == coll)
            {
                isPlayerInRange = true;
                break;
            }
        }
        // 플레이어가 범위 내에 있을 경우에만 데미지 적용
        if (isPlayerInRange)
        {
            player.Damage_HP(damage);
        }

        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(2f);
        nmove = true;
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
