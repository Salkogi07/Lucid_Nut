using UnityEngine;
using System.Collections;

public class RainEnemy : MonoBehaviour
{
    public GameObject rainPrefab; // 빗물 오브젝트의 프리팹
    public float rainSpeed = 20f; // 빗물의 속도
    public int numberOfRains = 3; // 발사할 빗물의 수
    public float horizontalOffset = 1f; // 가로로 배치할 간격
    public float delayBeforeFire = 1.5f; // 발사 전 대기 시간 (1.5초)
    public float fireInterval = 1f; // 발사 간격 (1초)

    private Transform playerTransform; // 플레이어의 Transform
    private GameObject[] rainObjects; // 소환된 빗물 오브젝트들
    public bool RS = false; // 발사 활성화 상태를 관리하는 변수

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
        // 플레이어가 존재하고 RS가 true일 때 발사
        if (playerTransform != null && RS)
        {
            Debug.Log("비 발사");
            ShootRain();
            RS = false; // 발사 후 RS를 false로 설정
        }
    }

    void ShootRain()
    {
        if (rainPrefab != null && playerTransform != null)
        {
            Vector3 shooterPosition = transform.position;

            // 빗물 오브젝트를 한 번에 소환
            rainObjects = new GameObject[numberOfRains];
            for (int i = 0; i < numberOfRains; i++)
            {
                // 겹치지 않게 가로로 오프셋을 적용하여 빗물 오브젝트 위치 설정
                Vector3 spawnPosition = shooterPosition + new Vector3(horizontalOffset * (i - (numberOfRains - 1) / 2f), 1 + (i * 0.5f), 0);

                // 빗물 오브젝트 생성
                rainObjects[i] = Instantiate(rainPrefab, spawnPosition, Quaternion.identity);
            }

            // 발사 전 대기 시간 후 발사 시작
            StartCoroutine(FireRainsAfterDelay());
        }
    }

    IEnumerator FireRainsAfterDelay()
    {
        // 발사 전 대기 시간
        yield return new WaitForSeconds(delayBeforeFire);

        foreach (GameObject rain in rainObjects)
        {
            if (rain != null)
            {
                // 빗물 오브젝트의 방향을 플레이어 방향으로 설정
                Vector3 spawnPosition = rain.transform.position;
                Vector3 direction = (playerTransform.position - spawnPosition).normalized;
                rain.GetComponent<Rigidbody2D>().velocity = direction * rainSpeed;

                // 다음 빗물 발사 간격을 적용
                yield return new WaitForSeconds(fireInterval);
            }
        }
    }

    // RS 값을 외부에서 설정할 수 있도록 public 메서드 제공
    public void ActivateRain()
    {
        RS = true; // 외부에서 호출 시 RS를 true로 설정
    }
}
