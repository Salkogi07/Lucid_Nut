using UnityEngine;

public class RainEnemy : MonoBehaviour
{
    public GameObject rainPrefab; // 빗물 오브젝트의 프리팹
    public float cooldownTime = 5f; // 쿨타임 (5초)
    public float rainSpeed = 10f; // 빗물의 속도
    public int numberOfRains = 10; // 발사할 빗물의 수

    private float lastShootTime = 0f; // 마지막 발사 시간
    private Transform playerTransform; // 플레이어의 Transform

    void Start()
    {
        // 태그가 "Player"인 오브젝트를 찾습니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // 현재 시간과 마지막 발사 시간의 차이를 확인하여 쿨타임이 지났는지 체크
        if (Time.time - lastShootTime >= cooldownTime)
        {
            // 플레이어가 존재하는지 확인
            if (playerTransform != null)
            {
                ShootRain();
                lastShootTime = Time.time; // 발사 시간 업데이트
            }
        }
    }

    void ShootRain()
    {
        if (rainPrefab != null)
        {
            Vector3 shooterPosition = transform.position;

            for (int i = 0; i < numberOfRains; i++)
            {
                // 빗물 오브젝트 생성
                GameObject rain = Instantiate(rainPrefab, shooterPosition, Quaternion.identity);

                // 빗물 오브젝트의 방향을 플레이어 방향으로 설정
                Vector3 direction = (playerTransform.position - shooterPosition).normalized;
                rain.GetComponent<Rigidbody2D>().velocity = direction * rainSpeed;
            }
        }
    }
}
