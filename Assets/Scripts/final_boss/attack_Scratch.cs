using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_Scratch : MonoBehaviour
{
    public GameObject alternateObject; // 교체할 오브젝트
    public float delayBeforeSwap = 1f; // 교체하기 전에 기다릴 시간

    void Start()
    {
        // 교체를 시작하는 코루틴 호출
        StartCoroutine(SwapObjectAfterDelay(delayBeforeSwap));
    }

    IEnumerator SwapObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간 동안 대기

        // 현재 오브젝트의 위치와 회전을 저장
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // 현재 오브젝트를 제거
        Destroy(gameObject);

        // 교체할 오브젝트를 현재 위치와 회전으로 소환
        Instantiate(alternateObject, currentPosition, currentRotation);
    }
}
