using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class darkness_unfolds2 : MonoBehaviour
{
    public GameObject lightObject; // Light 2D 컴포넌트를 포함하는 오브젝트
    public darkness_unfolds mainLightController; // 메인 라이트 컨트롤러 참조
    public float fadeDuration = 2.0f; // 서서히 변하는 시간 (초)
    public float darkDuration = 10.0f; // 어두운 상태 유지 시간 (초)

    private Light2D light2D;
    private float originalIntensity;

    private void Start()
    {
        // lightObject에서 Light 2D 컴포넌트 가져오기
        if (lightObject != null)
        {
            light2D = lightObject.GetComponent<Light2D>();
            if (light2D != null)
            {
                originalIntensity = light2D.intensity;
            }
            else
            {
                Debug.LogError("Light 2D component not found on the assigned object.");
            }
        }
        else
        {
            Debug.LogError("Light object is not assigned.");
        }
    }

    private void Update()
    {
        // 메인 라이트 컨트롤러의 Darkness_Unfolds 변수가 true로 변경되면 코루틴 시작
        if (mainLightController.Darkness_Unfolds && light2D != null)
        {
            StartCoroutine(BrightenLight());
        }
    }

    private IEnumerator BrightenLight()
    {
        // 현재 밝기에서 원래 밝기까지 서서히 증가
        float startIntensity = light2D.intensity;
        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            light2D.intensity = Mathf.Lerp(startIntensity, originalIntensity, t / fadeDuration);
            yield return null;
        }
        light2D.intensity = originalIntensity; // 확실하게 원래 밝기로 설정

        // 밝은 상태 유지 시간 대기
        yield return new WaitForSeconds(darkDuration);

        // 원래 밝기에서 0까지 서서히 줄이기
        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            light2D.intensity = Mathf.Lerp(originalIntensity, 0, t / fadeDuration);
            yield return null;
        }
        light2D.intensity = 0; // 확실하게 0으로 설정
    }
}
