using UnityEngine;

public class followingBG : MonoBehaviour
{
    public float smoothSpeed = 0.125f; // 배경이 따라오는 속도

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPosition = player.transform.position;
        Vector2 backgroundPosition = this.transform.position;

        // Time.deltaTime을 사용해 프레임에 독립적인 보간 계산
        backgroundPosition.x = Mathf.Lerp(backgroundPosition.x, playerPosition.x, smoothSpeed * Time.deltaTime);

        // 배경의 위치를 업데이트
        this.transform.position = backgroundPosition;
    }
}
