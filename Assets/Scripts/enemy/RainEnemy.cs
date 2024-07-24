using UnityEngine;
using System.Collections;
using System;

public class RainEnemy : MonoBehaviour
{
    public GameObject rainPrefab; // 빗물 오브젝트의 프리팹
    public float cooldownTime = 10f; // 쿨타임 (10초)
    public float rainSpeed = 20f; // 빗물의 속도
    public int numberOfRains = 3; // 발사할 빗물의 수
    public float horizontalOffset = 1f; // 가로로 배치할 간격
    public float delayBeforeFire = 1.5f; // 발사 전 대기 시간 (2초)
    public float fireInterval = 1f; // 발사 간격 (1.5초)

    private float lastShootTime = 0f; // 마지막 발사 시간
    private Transform playerTransform; // 플레이어의 Transform

    private GameObject[] rainObjects; // 소환된 빗물 오브젝트들

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
        if (rainPrefab != null && playerTransform != null)
        {
            Vector3 shooterPosition = transform.position;

            // 빗물 오브젝트를 한 번에 소환
            rainObjects = new GameObject[numberOfRains];
            for (int i = 0; i < numberOfRains; i++)
            {
                // 겹치지 않게 가로로 오프셋을 적용하여 빗물 오브젝트 위치 설정
                Vector3 spawnPosition = shooterPosition + new Vector3(horizontalOffset * (i - (numberOfRains - 1) / 2f), 1+(i*0.5f), 0);

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

                // 1.5초 후에 다음 빗물 발사
                yield return new WaitForSeconds(fireInterval);
            }
        }
    }
}
