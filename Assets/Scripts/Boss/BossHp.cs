using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    [SerializeField] private int hp = 0;
    [SerializeField] private int maxHp = 5000;

    public Image healthSlider;
    private Coroutine healthCoroutine;

    private void Start()
    {
        hp = maxHp;
        healthSlider.fillAmount = 1f; // 슬라이더는 1이 max로 시작하므로 1로 초기화
    }

    // 플레이어가 공격할 때 호출되는 함수
    public void TakeDamage(int damage)
    {
        // 실제 체력은 즉시 감소
        hp = Mathf.Clamp(hp - damage, 0, maxHp);

        // 만약 이전에 체력 감소 코루틴이 실행 중이면 중지
        if (healthCoroutine != null)
        {
            StopCoroutine(healthCoroutine);
        }

        // 슬라이더 감소를 부드럽게 처리하는 코루틴 실행
        healthCoroutine = StartCoroutine(SmoothHealthDecrease(hp));
    }

    // 부드럽게 슬라이더가 감소하는 코루틴
    private IEnumerator SmoothHealthDecrease(int targetHealth)
    {
        float elapsedTime = 0f;
        float duration = 0.5f; // 슬라이더가 감소하는데 걸리는 시간 (0.5초)

        float startingFill = healthSlider.fillAmount; // 슬라이더의 현재 fill 값
        float targetFill = (float)targetHealth / maxHp; // 목표 슬라이더 값

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // 슬라이더 값을 부드럽게 줄어들도록 보간함
            healthSlider.fillAmount = Mathf.Lerp(startingFill, targetFill, elapsedTime / duration);
            yield return null; // 한 프레임 대기
        }

        // 슬라이더 감소 완료 후 정확하게 타겟 체력으로 맞춰줌
        healthSlider.fillAmount = targetFill;
    }
}
