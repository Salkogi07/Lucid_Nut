using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 2f;  // 투명해지는 데 걸리는 시간
    private Renderer objectRenderer;
    private Color originalColor;
    private float fadeTimer;

    void Start()
    {
        // Renderer 컴포넌트를 가져오기
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            // 현재 머티리얼의 색상 가져오기
            originalColor = objectRenderer.material.color;
        }
        else
        {
            Debug.LogError("Renderer component not found on the object.");
        }
    }

    void Update()
    {
        if (objectRenderer != null)
        {
            // 타이머 증가
            fadeTimer += Time.deltaTime;

            // 투명도 계산
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);

            // 머티리얼의 색상 업데이트
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            objectRenderer.material.color = newColor;

            // 완전히 투명해지면 오브젝트를 삭제
           /* if (alpha <= 0f)
            {
                Destroy(gameObject); // 오브젝트 삭제
            }*/
        }
    }

    public void StartFading()
    {
        // 투명화 시작
        fadeTimer = 0f;
    }
}
