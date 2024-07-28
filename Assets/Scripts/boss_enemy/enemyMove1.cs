using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMove2 : MonoBehaviour
{
    public float speed = 2f; // 적의 이동 속도
    public float dashSpeed = 20f; // 대쉬 속도
    public float cooldownTime = 5f; // 쿨타임 시간
    private Transform player; // 플레이어의 트랜스폼
    private Vector2 dashTarget; // 대쉬 목표 위치
    private bool isCooldown = false; // 대쉬 쿨타임 여부
    private bool isDashing = false; // 대쉬 중 여부
    private bool isPreparingToDash = false; // 대쉬 준비 중 여부

    void Start()
    {
        // 태그가 "Player"인 객체를 찾습니다.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // 플레이어 객체가 존재하는지 확인하고 트랜스폼을 저장합니다.
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        // 플레이어가 존재하고 대쉬 중이 아니며 대쉬 준비 중이 아닐 때만 적이 움직이도록 합니다.
        if (player != null && !isDashing && !isPreparingToDash)
        {
            // 플레이어와 적 사이의 방향을 계산합니다.
            Vector2 direction = player.position - transform.position;
            direction.Normalize();

            // 적을 플레이어 방향으로 이동시킵니다.
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 콜라이더 안에 태그가 "Player"인 객체가 들어왔는지 확인합니다.
        if (other.CompareTag("Player") && !isCooldown && !isPreparingToDash)
        {
            StartCoroutine(PrepareAndDash(other.transform));
        }
    }

    private IEnumerator PrepareAndDash(Transform playerTransform)
    {
        // 대쉬 준비 중으로 설정
        isPreparingToDash = true;

        // 0.5초 동안 멈춥니다.
        yield return new WaitForSeconds(0.5f);

        // 플레이어의 현재 위치를 저장합니다.
        dashTarget = playerTransform.position;

        // 대쉬하는 동안 대쉬 중으로 설정합니다.
        isDashing = true;
        isPreparingToDash = false;

        // 대쉬하는 동안 플레이어 위치로 이동합니다.
        while (Vector2.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null;
        }

        // 대쉬가 끝난 후 쿨타임을 시작합니다.
        isCooldown = true;
        isDashing = false;
        yield return new WaitForSeconds(cooldownTime);

        // 쿨타임 종료
        isCooldown = false;
    }
}