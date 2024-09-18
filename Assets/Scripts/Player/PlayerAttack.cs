// PlayerAttack.cs 수정 코드
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerMove playerMove;  // PlayerMove 스크립트를 참조

    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;  // 이펙트가 생성될 고정 위치
    public Vector2 boxSize;
    public GameObject AttackHitEffect;
    public GameObject SlashEffect;    // 휘두르는 이펙트
    public AudioClip attackSound;     // 공격 사운드 클립 추가
    private AudioSource audioSource;  // 오디오 소스 추가

    // 마우스 드래그 공격 추가
    private Vector3 startMousePosition;
    private Vector3 endMousePosition;
    private bool isDragging = false;

    public float minDragDistance = 0.5f; // 최소 드래그 거리
    public float maxDragDistance = 5.0f; // 최대 드래그 거리
    public float minDamage = 10f;  // 최소 데미지
    public float maxDamage = 50f;  // 최대 데미지
    public float minSlashSize = 1f; // 최소 슬래시 크기
    public float maxSlashSize = 3f; // 최대 슬래시 크기

    private LineRenderer lineRenderer;  // 라인 렌더러 추가

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();  // PlayerMove 스크립트 가져오기
        audioSource = GetComponent<AudioSource>();  // 오디오 소스 컴포넌트 가져오기

        if (audioSource == null)
        {
            // 만약 오디오 소스가 없으면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // LineRenderer 설정
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;  // 시작점과 끝점, 두 개의 점
        lineRenderer.startWidth = 0.1f;  // 선의 시작점 두께
        lineRenderer.endWidth = 0.1f;    // 선의 끝점 두께
        lineRenderer.enabled = false;    // 기본적으로 비활성화
    }

    void Update()
    {
        if (curTime <= 0)
        {
            // 드래그 공격 방식 (마우스 클릭 시작)
            if (Input.GetMouseButtonDown(0) && !isDragging)
            {
                startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                startMousePosition.z = 0; // 2D 환경이므로 z 축 고정
                isDragging = true;

                // 드래그 시작 시점에서 라인 렌더러 활성화
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, startMousePosition);  // 시작점 설정
            }

            // 드래그 중일 때 라인 업데이트
            if (isDragging)
            {
                Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentMousePosition.z = 0;

                // 마우스 시작 위치와 현재 위치 간의 거리 계산
                float dragDistance = Vector3.Distance(startMousePosition, currentMousePosition);

                // 거리를 최소 및 최대 값으로 제한
                dragDistance = Mathf.Clamp(dragDistance, minDragDistance, maxDragDistance);

                // 제한된 거리만큼 드래그된 위치 계산
                Vector3 clampedPosition = startMousePosition + (currentMousePosition - startMousePosition).normalized * dragDistance;

                lineRenderer.SetPosition(1, clampedPosition);  // 제한된 거리에 맞춰 끝점 업데이트
            }

            // 드래그 공격 종료 시점 (마우스 클릭을 떼는 순간)
            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                endMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                endMousePosition.z = 0;
                isDragging = false;

                // 드래그 거리 계산
                float dragDistance = Vector2.Distance(startMousePosition, endMousePosition);
                dragDistance = Mathf.Clamp(dragDistance, minDragDistance, maxDragDistance); // 드래그 거리 제한

                // 스크린 기준 좌우 방향을 기준으로 공격 방향 결정
                Vector2 direction = (startMousePosition.x < Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0)).x) ? Vector2.left : Vector2.right;

                // 슬래시 공격 수행 (방향, 크기 적용)
                PerformSlashAttack(dragDistance, direction);
                curTime = coolTime; // 쿨타임 적용

                // 드래그 끝났으므로 라인 렌더러 비활성화
                lineRenderer.enabled = false;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    private void PerformSlashAttack(float dragDistance, Vector2 direction)
    {
        playerMove.isAttack = true;

        // 드래그 방향에 따른 각도 설정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // 드래그한 방향에 맞춰 각도 계산

        Vector3 middlePoint = (startMousePosition + endMousePosition) / 2;
        Vector3 angleVec = (middlePoint - transform.position).normalized;
        float anglePos = Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg;
        Quaternion lookRotation = Quaternion.AngleAxis(anglePos, Vector3.forward);

        playerMove.FlipAttack(direction.x);  // 플레이어의 방향을 드래그한 방향으로 설정

        // 드래그 거리 기반으로 데미지 및 슬래시 크기 계산
        float damage = Mathf.Lerp(maxDamage, minDamage, (dragDistance - minDragDistance) / (maxDragDistance - minDragDistance));
        float slashSize = Mathf.Lerp(minSlashSize, maxSlashSize, (dragDistance - minDragDistance) / (maxDragDistance - minDragDistance));

        GameObject SE = Instantiate(SlashEffect,pos.position,lookRotation, transform);  // 부모 객체로 플레이어 설정

        SE.transform.localScale = new Vector3(direction.x < 0 ? -slashSize : slashSize, slashSize, 1);  // 슬래시 크기를 라인의 길이에 맞게 조정

        Destroy(SE, 0.3f);  // 짧은 시간 후 이펙트 삭제

        // 슬래시 범위 내 적 공격 (이펙트 크기와 동일하게 범위 설정)
        Vector2 hitBoxSize = boxSize * slashSize;
        Vector2 hitBoxCenter = pos.position;  // 고정된 위치에서 히트박스 생성

        audioSource.PlayOneShot(attackSound);  // 공격할 때 사운드 재생

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(hitBoxCenter, hitBoxSize, angle);  // 슬래시 크기 및 방향 반영
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyHP enemy = collider.GetComponent<EnemyHP>();
                if (enemy != null)
                    enemy.TakeDamage(damage);  // 드래그 거리 기반 데미지 적용

                float effectDirection = playerMove.isFacingRight ? 1f : -1f;
                Vector3 effectScale = new Vector3(effectDirection, 1, 1);
                GameObject AE = Instantiate(AttackHitEffect, collider.transform.position, Quaternion.identity);
                AE.transform.localScale = effectScale;
                Destroy(AE, 0.5f);
            }
            else if (collider.CompareTag("Boss"))
            {
                BossHp boss = collider.GetComponent<BossHp>();
                if (boss != null)
                    boss.TakeDamage((int)damage);  // 드래그 거리 기반 데미지 적용

                float effectDirection = playerMove.isFacingRight ? 1f : -1f;
                Vector3 effectScale = new Vector3(effectDirection, 1, 1);
                GameObject AE = Instantiate(AttackHitEffect, collider.transform.position, Quaternion.identity);
                AE.transform.localScale = effectScale;
                Destroy(AE, 0.5f);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (pos == null)
            return;

        // 슬래시 크기를 실시간으로 계산 (실제 공격 시와 동일하게 적용)
        float dragDistance = Vector2.Distance(startMousePosition, endMousePosition);
        float slashSize = Mathf.Lerp(minSlashSize, maxSlashSize, (dragDistance - minDragDistance) / (maxDragDistance - minDragDistance));

        // OverlapBox 크기와 위치
        Vector2 hitBoxSize = boxSize * slashSize;
        Vector2 hitBoxCenter = pos.position;

        // Gizmos 색상 설정 (빨간색)
        Gizmos.color = Color.red;

        // OverlapBox 범위를 그리기
        Gizmos.DrawWireCube(hitBoxCenter, hitBoxSize);
    }

}
