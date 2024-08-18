using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_fire : MonoBehaviour
{
    public GameObject objectToSpawn; // 소환할 오브젝트 프리팹
    public int numberOfObjects = 3; // 소환할 오브젝트 개수
    public BoxCollider2D spawnArea; // 소환 영역 콜라이더
    public bool LS = false; // LS 변수 추가

    // 위치를 저장할 리스트를 클래스 레벨에 정의
    private List<Vector2> spawnPositions = new List<Vector2>();

    void Update()
    {
        if (LS) // LS가 true일 때만 실행
        {
            SpawnObjects();
            LS = false; // 실행 후 LS를 false로 설정
        }
    }

    void SpawnObjects()
    {
        Bounds bounds = spawnArea.bounds;
        int objectsSpawned = 0;

        while (objectsSpawned < numberOfObjects)
        {
            Vector2 spawnPosition;
            int attempts = 0; // 시도 횟수

            // 유효한 위치를 찾을 때까지 반복
            while (attempts < 100)
            {
                attempts++; // 시도 횟수 증가
                spawnPosition = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y)
                );

                // 모든 위치를 소환 가능하도록 설정
                spawnPositions.Add(spawnPosition); // 위치를 리스트에 추가
                Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                objectsSpawned++;
                break; // 유효한 위치가 확인되면 루프 종료
            }
            // 100번의 시도 후에도 유효한 위치를 찾지 못한 경우 경고 메시지 출력
            if (attempts >= 100)
            {
                Debug.LogWarning("Could not find suitable spawn position after 100 attempts.");
                break;
            }
        }
    }
}