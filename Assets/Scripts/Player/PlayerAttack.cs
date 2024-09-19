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
    public GameObject SlashEffect;    // 휘두르는 이펙트
    public GameObject StingEffect;    // 찌르기 이펙트

    public GameObject AttackHitEffect;
    public AudioClip attackSound;     // 공격 사운드 클립 추가
    private AudioSource audioSource;  // 오디오 소스 추가

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();  // PlayerMove 스크립트 가져오기
        audioSource = GetComponent<AudioSource>();  // 오디오 소스 컴포넌트 가져오기

        if (audioSource == null)
        {
            // 만약 오디오 소스가 없으면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (curTime <= 0)
        {
            // 마우스 좌클릭 (휘두르기 공격)
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;  // 2D 환경이므로 z 축 고정

                Vector2 direction = (mousePosition.x < transform.position.x) ? Vector2.left : Vector2.right;

                PerformSlashAttack(direction);  // 휘두르기 공격 수행
                curTime = coolTime;  // 쿨타임 적용
            }

            // 마우스 우클릭 (찌르기 공격)
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;  // 2D 환경이므로 z 축 고정

                Vector2 direction = (mousePosition.x < transform.position.x) ? Vector2.left : Vector2.right;

                PerformStingAttack(direction);  // 찌르기 공격 수행
                curTime = coolTime;  // 쿨타임 적용
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    private void PerformSlashAttack(Vector2 direction)
    {
        playerMove.isAttack = true;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // 공격 방향에 맞춰 각도 계산
        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        playerMove.FlipAttack(direction.x);  // 플레이어의 방향을 공격 방향으로 설정

        GameObject SE = Instantiate(SlashEffect, pos.position, lookRotation, transform);  // 휘두르는 이펙트 생성
        Destroy(SE, 0.3f);  // 짧은 시간 후 이펙트 삭제

        audioSource.PlayOneShot(attackSound);  // 공격할 때 사운드 재생

        // 슬래시 범위 내 적 공격
        Vector2 hitBoxSize = boxSize;
        Vector2 hitBoxCenter = pos.position;  // 고정된 위치에서 히트박스 생성

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(hitBoxCenter, hitBoxSize, angle);  // 슬래시 크기 및 방향 반영
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyHP enemy = collider.GetComponent<EnemyHP>();
                if (enemy != null)
                    enemy.TakeDamage(20f);  // 데미지 적용

                CreateHitEffect(collider.transform.position, direction.x);
            }
            else if (collider.CompareTag("Boss"))
            {
                BossHp boss = collider.GetComponent<BossHp>();
                if (boss != null)
                    boss.TakeDamage(20);  // 데미지 적용

                CreateHitEffect(collider.transform.position, direction.x);
            }
        }
    }

    private void PerformStingAttack(Vector2 direction)
    {
        playerMove.isAttack = true;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // 공격 방향에 맞춰 각도 계산
        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        playerMove.FlipAttack(direction.x);  // 플레이어의 방향을 공격 방향으로 설정

        GameObject SE = Instantiate(StingEffect, pos.position, lookRotation, transform);  // 찌르기 이펙트 생성
        Destroy(SE, 0.3f);  // 짧은 시간 후 이펙트 삭제

        audioSource.PlayOneShot(attackSound);  // 공격할 때 사운드 재생

        // 찌르기 범위 내 적 공격
        Vector2 hitBoxSize = boxSize;
        Vector2 hitBoxCenter = pos.position;  // 고정된 위치에서 히트박스 생성

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(hitBoxCenter, hitBoxSize, angle);  // 찌르기 크기 및 방향 반영
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyHP enemy = collider.GetComponent<EnemyHP>();
                if (enemy != null)
                    enemy.TakeDamage(30f);  // 데미지 적용

                CreateHitEffect(collider.transform.position, direction.x);
            }
            else if (collider.CompareTag("Boss"))
            {
                BossHp boss = collider.GetComponent<BossHp>();
                if (boss != null)
                    boss.TakeDamage(30);  // 데미지 적용

                CreateHitEffect(collider.transform.position, direction.x);
            }
        }
    }

    private void CreateHitEffect(Vector3 position, float direction)
    {
        float effectDirection = direction > 0 ? 1f : -1f;
        Vector3 effectScale = new Vector3(effectDirection, 1, 1);

        GameObject AE = Instantiate(AttackHitEffect, position, Quaternion.identity);
        AE.transform.localScale = effectScale;
        Destroy(AE, 0.5f);
    }

    private void OnDrawGizmos()
    {
        if (pos == null)
            return;

        // OverlapBox 크기와 위치
        Vector2 hitBoxSize = boxSize;
        Vector2 hitBoxCenter = pos.position;

        // Gizmos 색상 설정 (빨간색)
        Gizmos.color = Color.red;

        // OverlapBox 범위를 그리기
        Gizmos.DrawWireCube(hitBoxCenter, hitBoxSize);
    }
}
