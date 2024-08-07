using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class darkness_unfolds : MonoBehaviour
{
    public GameObject mainLightObject; // 메인 라이트 오브젝트
    public GameObject playerLightObject; // 플레이어 라이트 오브젝트
    public bool Darkness_Unfolds = false; // 외부에서 제어할 변수
    public float fadeDuration = 2.0f; // 서서히 변하는 시간 (초)
    public float darkDuration = 10.0f; // 어두운 상태 유지 시간 (초)
    public float playerTargetIntensity = 1.0f; // 플레이어 라이트의 최종 밝기 값

    private Light2D mainLight;
    private Light2D playerLight;
    private float mainOriginalIntensity;
    private float initialPlayerIntensity = 0.0f; // 플레이어 라이트의 초기 밝기 값 (0으로 설정)

    private void Start()
    {
        // 메인 라이트 오브젝트에서 Light 2D 컴포넌트 가져오기
        if (mainLightObject != null)
        {
            mainLight = mainLightObject.GetComponent<Light2D>();
            if (mainLight != null)
            {
                mainOriginalIntensity = mainLight.intensity;
            }
            else
            {
                Debug.LogError("Light 2D component not found on the main light object.");
            }
        }
        else
        {
            Debug.LogError("Main light object is not assigned.");
        }

        // 플레이어 라이트 오브젝트에서 Light 2D 컴포넌트 가져오기
        if (playerLightObject != null)
        {
            playerLight = playerLightObject.GetComponent<Light2D>();
            if (playerLight != null)
            {
                // 플레이어 라이트의 초기 밝기 설정
                playerLight.intensity = initialPlayerIntensity;
            }
            else
            {
                Debug.LogError("Light 2D component not found on the player light object.");
            }
        }
        else
        {
            Debug.LogError("Player light object is not assigned.");
        }
    }

    private void Update()
    {
        // Darkness_Unfolds 변수가 true로 변경되면 코루틴 시작
        if (Darkness_Unfolds && mainLight != null && playerLight != null)
        {
            Darkness_Unfolds = false; // 한번만 실행되도록 다시 false로 변경
            StartCoroutine(ControlLights());
        }
    }

    private IEnumerator ControlLights()
    {

        // 메인 라이트를 현재 밝기에서 0까지 서서히 줄이기
        // 플레이어 라이트를 초기 밝기에서 목표 밝기까지 서서히 증가
        float mainStartIntensity = mainLight.intensity;
        float playerStartIntensity = initialPlayerIntensity; // 플레이어 라이트의 시작 밝기
        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            mainLight.intensity = Mathf.Lerp(mainStartIntensity, 0, t / fadeDuration);
            playerLight.intensity = Mathf.Lerp(playerStartIntensity, playerTargetIntensity, t / fadeDuration);

            yield return null;
        }
        mainLight.intensity = 0; // 확실하게 0으로 설정
        playerLight.intensity = playerTargetIntensity; // 확실하게 목표 밝기로 설정

        // 어두운 상태 유지 시간 대기
        yield return new WaitForSeconds(darkDuration);

        // 메인 라이트를 0에서 원래 밝기까지 서서히 증가
        // 플레이어 라이트를 목표 밝기에서 0까지 서서히 줄이기
        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            mainLight.intensity = Mathf.Lerp(0, mainOriginalIntensity, t / fadeDuration);
            playerLight.intensity = Mathf.Lerp(playerTargetIntensity, 0, t / fadeDuration);

            yield return null;
        }
        mainLight.intensity = mainOriginalIntensity; // 확실하게 원래 밝기로 설정
        playerLight.intensity = 0; // 확실하게 0으로 설정
    }
}
