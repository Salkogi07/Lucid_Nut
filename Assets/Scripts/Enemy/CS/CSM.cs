using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CSM : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer; // SpriteRenderer 추가

    [Header("Move info")]
    private Rigidbody2D rigid;
    public bool move = true;
    public float moveSpeed = 3f;
    public int nextmove;

    [Header("Attack info")]
    public GameObject projectilePrefab; // 발사할 오브젝트의 프리팹
    public float projectileSpeed = 15f; // 발사 속도
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
        if (move) // 대쉬 중이 아닐 때만 이동
        {
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);
        }
        spriteRenderer.flipX = rigid.velocity.x > 0;
        DetectAndMoveTowardsPlayer();
        Attack();
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

    void Attack()
    {
        // 플레이어를 감지할 사각형 영역
        Collider2D[] players = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset1, detectionSize1, 0f, playerLayer1);

        foreach (Collider2D collider in players)
        {
            if (collider.tag == "Player" && attackC)
            {
                attackC = false;
                StartCoroutine(FireProjectile(collider.transform.position));
                break; // 감지된 플레이어가 있으면 한 번만 발사
            }
        }
    }

    private IEnumerator FireProjectile(Vector2 targetPosition)
    {
        animator.SetBool("Attack", true);
        // 발사체 생성
        nmove = true;
        nextmove = 0;
        yield return new WaitForSeconds(1f);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        targetPosition = (Vector2)players[0].transform.position;

        // 발사체의 Rigidbody2D 컴포넌트 가져오기
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            // 목표 위치로의 방향 계산
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            projectileRb.velocity = direction * projectileSpeed;
        }
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(1f);
        nmove = false;
        nextmove = Random.Range(-1, 2);
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