using UnityEngine;

public class Chaser : MonoBehaviour
{
    public float moveSpeed = 13f; // 이동 속도 조정 가능
    private Transform playerTransform;
    private Vector3 targetPosition;
    private bool hasTarget = false;

    void Start()
    {
        // "Player" 태그를 가진 오브젝트를 찾습니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            hasTarget = true;
        }
        else
        {
            Debug.LogWarning("No object with tag 'Player' found.");
        }

        // 1초마다 UpdateTargetPosition 메서드를 호출합니다.
        InvokeRepeating("UpdateTargetPosition", 0f, 2f);
    }

    void Update()
    {
        if (hasTarget)
        {
            // 현재 위치와 목표 위치 사이의 방향을 계산합니다.
            Vector3 direction = (targetPosition - transform.position).normalized;

            // 방향 벡터를 기반으로 오브젝트를 회전시킵니다.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // 이동할 방향으로 이동합니다.
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void UpdateTargetPosition()
    {
        if (playerTransform != null)
        {
            // 목표 위치를 현재 플레이어의 위치로 업데이트합니다.
            targetPosition = playerTransform.position;
        }
    }
}
