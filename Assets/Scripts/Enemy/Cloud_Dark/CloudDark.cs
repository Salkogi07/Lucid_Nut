using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CloudDark : MonoBehaviour
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
    public Vector2 detectionSize1 = new Vector2(5f, 2f); // 플레이어 감지 범위 (사각형 크기)
    public Vector2 detectionOffset1 = Vector2.zero; // 사각형의 위치 오프셋
    public LayerMask playerLayer1; // 플레이어 레이어
    public GameObject dashEffect;
    public GameObject projectilePrefab; // 발사체 프리팹
    public GameObject projectilePrefab2; // 발사체 프리팹
    public int dashCount = 0; // 대쉬 횟수
    private bool hasFiredProjectile = false; // 발사체 발사 여부

    [Header("Detection info")]
    public Vector2 detectionSize = new Vector2(15f, 5f); // 플레이어 감지 범위 (사각형 크기)
    public Vector2 detectionOffset = Vector2.zero; // 사각형의 위치 오프셋
    public LayerMask playerLayer; // 플레이어 레이어
    public bool nmove = true;

    private Transform playerTransform;
    private Vector2 dashStartPosition; // 대쉬 시작 위치
    [SerializeField] private float dashDistance = 10f; // 대쉬 거리

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기
        think();
    }

    private void Update()
    {
        DetectAndMoveTowardsPlayer();
        Dashcoll();
        if (move && !isDashing) // 대쉬 중이 아닐 때만 이동
        {
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);
            spriteRenderer.flipX = rigid.velocity.x > 0;
        }
    }

    void think()
    {
        if (nmove)
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
                dashStartPosition = transform.position; // 대쉬 시작 위치 저장
                dashTarget = collider.transform.position; // 대쉬 목표 설정
                dashTarget.y = transform.position.y; // y축 자기걸로 변경

                if (dashCount < 3 && !isDashing)
                {
                    StartCoroutine(PrepareAndDash(dashTarget));
                    dashCount++;
                }
                else if (!hasFiredProjectile) // 발사체가 아직 발사되지 않았으면
                {
                    isDashing = true;
                    StartCoroutine(LaunchProjectile(dashTarget));
                    dashCount = 0; // 대쉬 횟수 초기화
                    hasFiredProjectile = true; // 발사체 발사 상태 설정
                }
            }
        }
    }

    private IEnumerator PrepareAndDash(Vector2 playerPosition)
    {
        move = false;
        nmove = false;
        isPreparingToDash = true; // 대쉬 준비 중으로 설정
        isDashing = true; // 대쉬 시작
        animator.SetBool("Attack", true);

        // 플레이어 방향 벡터 계산
        Vector2 directionToPlayer = (playerPosition - (Vector2)transform.position).normalized;
        dashTarget = (Vector2)transform.position + directionToPlayer * dashDistance; // 대쉬 목표 설정

        // 스프라이트 방향 설정
        spriteRenderer.flipX = directionToPlayer.x > 0;

        yield return new WaitForSeconds(0.5f); // 준비 시간 동안 대기

        GameObject explosion = Instantiate(dashEffect, this.transform.position, Quaternion.identity);
        Destroy(explosion, 0.5f);

        // 대쉬 중 목표 위치로 이동
        while (Vector2.Distance(transform.position, dashTarget) > 0.2f)
        {
            // 대쉬 목표 방향으로 이동
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

            // 대쉬 중에도 스프라이트 방향 조정
            Vector2 direction = (dashTarget - (Vector2)transform.position).normalized;
            spriteRenderer.flipX = direction.x > 0;

            yield return null; // 프레임마다 대기
        }

        // 대쉬 완료 후 방향 조정
        animator.SetBool("Attack", false);
        isDashing = false; // 대쉬 종료
        yield return new WaitForSeconds(1f); // 쿨타임 대기
        move = true;
        nmove = true;
        isPreparingToDash = false; // 대쉬 준비 완료
        hasFiredProjectile = false; // 다음 발사를 위해 발사 상태 초기화
    }

    private IEnumerator LaunchProjectile(Vector2 targetPosition)
    {
        Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;
        spriteRenderer.flipX = directionToPlayer.x > 0;

        move = false;
        nmove = false;

        if (projectilePrefab != null)
        {
            animator.SetBool("Attack2", true);
            yield return new WaitForSeconds(1.3f);

            // 발사체 인스턴스화 및 초기화
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (projectileRb != null)
            {
                // 발사체를 위로 발사
                Vector2 launchDirection = new Vector2(0, 1); // 위쪽 방향
                float launchSpeed = 30f; // 발사 속도

                projectileRb.velocity = launchDirection * launchSpeed;
                animator.SetBool("Attack2", false);

                // 2초간 X축만 따라다니기
                float timeElapsed = 0f;
                float fixedHeight = 20f; // 일정 고도

                while (timeElapsed < 1f)
                {
                    if (projectile.transform.position.y >= fixedHeight)
                    {
                        // 발사체의 Y축 위치를 고정
                        Vector2 currentPosition = projectile.transform.position;
                        currentPosition.y = fixedHeight;
                        projectile.transform.position = currentPosition;
                    }

                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                Vector2 playerpos = player.transform.position;
                Vector2 currpos = projectile.transform.position;
                currpos.x = playerpos.x;
                projectile.transform.position = currpos;

                // 발사체가 떨어질 때까지 대기
                while (projectileRb.velocity.y > 0)
                {
                    yield return null;
                }

                // 발사체 삭제 전, 여러 오브젝트를 생성하여 아래로 떨어뜨리기
                int numberOfObjects = 8; // 떨어뜨릴 오브젝트 개수
                for (int i = 0; i < numberOfObjects; i++)
                {
                    Vector2 spawnPosition = new Vector2(projectile.transform.position.x, projectile.transform.position.y);
                    GameObject fallingObject = Instantiate(projectilePrefab2, spawnPosition, Quaternion.identity);
                    Rigidbody2D fallingObjectRb = fallingObject.GetComponent<Rigidbody2D>();

                    if (fallingObjectRb != null)
                    {
                        Vector2 fallingDirection = new Vector2(Random.Range(-3f, 3f), -1); // 약간의 좌우 랜덤 방향으로 떨어뜨리기
                        fallingObjectRb.velocity = fallingDirection.normalized * Random.Range(2f, 5f); // 떨어지는 속도 설정
                    }
                    Destroy(fallingObject,4f);
                }

                Destroy(projectile); // 원래 발사체 삭제

                yield return new WaitForSeconds(1f); // 대쉬 종료 대기
                isDashing = false;
                move = true;
                nmove = true;
                hasFiredProjectile = false;
            }
        }
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
