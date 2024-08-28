using UnityEngine;

public class followingBG : MonoBehaviour
{
    public float smoothSpeed = 0.125f; // 배경이 따라오는 속도

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPosition = player.transform.position;
        Vector3 backgroundPosition = this.transform.position;

        // x축과 y축 모두 부드럽게 따라오도록 업데이트
        backgroundPosition.x = Mathf.Lerp(backgroundPosition.x, playerPosition.x, smoothSpeed * Time.deltaTime);
        backgroundPosition.y = Mathf.Lerp(backgroundPosition.y, playerPosition.y, smoothSpeed * Time.deltaTime);

        backgroundPosition.z = 2f; // z축 고정
        this.transform.position = backgroundPosition;
    }
}
