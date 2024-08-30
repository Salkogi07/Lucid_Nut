using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public Image Panel;
    public AudioSource audioSource; // 오디오 소스 추가
    float time = 0f;
    public float Fade_time = 1f;
    public float PitchFade_time = 1f; // 피치 페이드 시간 추가

    public void StartPrologue()
    {
        StartCoroutine(FadeFlow());
    }

    public void EndGame()
    {
        Application.Quit();
    }

    IEnumerator FadeFlow()
    {
        Color alpha = Panel.color;

        // 페이드 인 및 피치 감소를 동시에 시작
        time = 0f;
        Panel.gameObject.SetActive(true);
        float pitchTime = 0f;

        while (alpha.a < 1f || audioSource.volume > 0f)
        {
            time += Time.deltaTime / Fade_time;
            pitchTime += Time.deltaTime / PitchFade_time;

            // 패널의 알파값을 점차 증가시킴 (페이드 인)
            if (alpha.a < 1f)
            {
                alpha.a = Mathf.Lerp(0, 1, time);
                Panel.color = alpha;
            }

            // 오디오 피치와 볼륨을 점차 감소시킴
            if (audioSource.volume > 0f)
            {
                audioSource.volume = Mathf.Lerp(1f, 0f, pitchTime);
            }

            yield return null;
        }

        // 페이드와 피치 감소가 모두 완료된 후 씬 전환
        SceneManager.LoadScene(1);
        yield return null;
    }
}
