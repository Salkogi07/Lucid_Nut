using System.Collections;
using UnityEngine;

public class dash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float cooldownTime = 5f;

    public bool DS = false; // 대쉬 활성화 여부를 제어하는 변수

    private bool isCooldown = false; // 대쉬 쿨타임 여부
    public bool isDashing = false; // 대쉬 중 여부
    public bool isPreparingToDash = false; // 대쉬 준비 중 여부

    private Vector2 dashTarget; // 대쉬 목표 위치

    // 대쉬 시작
    public void TryStartDash(Vector2 targetPosition)
    {
        if (DS && !isCooldown && !isDashing) // DS가 true일 때만 대쉬를 시도
        {
            StartCoroutine(PrepareAndDash(targetPosition));
            DS = false; // 대쉬를 시작한 후 DS를 false로 설정
        }
    }

    private IEnumerator PrepareAndDash(Vector2 targetPosition)
    {
        isPreparingToDash = true; // 대쉬 준비 중으로 설정
        yield return new WaitForSeconds(0.5f); // 준비 시간 동안 대기

        dashTarget = targetPosition; // 대쉬 목표 설정
        isPreparingToDash = false; // 대쉬 준비 완료
        isDashing = true; // 대쉬 시작

        // 대쉬 중 목표 위치로 이동
        while (Vector2.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null; // 프레임마다 대기
        }

        isDashing = false; // 대쉬 종료
        isCooldown = true; // 쿨타임 시작
        yield return new WaitForSeconds(cooldownTime); // 쿨타임 대기
        isCooldown = false; // 쿨타임 종료
    }

    // 대쉬 상태를 확인하는 프로퍼티
    public bool IsDashing { get { return isDashing; } }
    public bool IsPreparingToDash { get { return isPreparingToDash; } }
    public bool IsCooldown { get { return isCooldown; } }
}
