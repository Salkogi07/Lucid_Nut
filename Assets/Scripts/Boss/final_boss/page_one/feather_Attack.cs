using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feather_Attack : MonoBehaviour
{
    public GameObject objectToSpawn; // 소환할 오브젝트
    public float spawnInterval = 1f; // 오브젝트를 소환할 간격 (각 소환 간격)
    public int numberOfSpawns = 5; // 소환할 오브젝트 수
    public float delayBetweenSpawns = 5f; // 두 번째 소환까지의 대기 시간
    public Collider2D spawnAreaCollider; // 소환 영역을 정의하는 콜라이더
    public bool feather_attack = false; // 소환을 제어하는 변수
    public float minDistanceBetweenObjects = 3f; // 오브젝트 간 최소 간격

    private List<Vector2> occupiedPositions = new List<Vector2>(); // 소환된 오브젝트의 위치 목록

    private void Update()
    {
        // feather_attack이 true일 때만 소환을 시작
        if (feather_attack)
        {
            StartCoroutine(SpawnObjects());
            feather_attack = false; // 소환 후에는 feather_attack을 false로 리셋
        }
    }

    private IEnumerator SpawnObjects()
    {
        // 첫 번째 소환
        for (int i = 0; i < numberOfSpawns; i++)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }

        // 첫 번째 소환 후 대기
        yield return new WaitForSeconds(delayBetweenSpawns);

        occupiedPositions = new List<Vector2>();

        // 두 번째 소환
        for (int i = 0; i < numberOfSpawns; i++)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn == null || spawnAreaCollider == null)
        {
            Debug.LogWarning("Object to spawn or spawn area collider not set.");
            return;
        }

        Vector2 spawnPosition = Vector2.zero; // 변수 초기화
        bool validPosition = false;

        // 최대 100번의 시도를 통해 유효한 위치를 찾습니다
        for (int attempt = 0; attempt < 100; attempt++)
        {
            // 콜라이더 영역 내에서 랜덤 위치 계산
            spawnPosition = GetRandomPositionInCollider(spawnAreaCollider);

            // 위치가 유효한지 확인
            if (IsValidPosition(spawnPosition))
            {
                validPosition = true;
                break;
            }
        }

        if (validPosition)
        {
            // 오브젝트 생성
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            occupiedPositions.Add(spawnPosition); // 생성된 위치를 기록합니다
        }
        else
        {
            Debug.LogWarning("Failed to find a valid spawn position.");
        }
    }

    private Vector2 GetRandomPositionInCollider(Collider2D collider)
    {
        // 콜라이더의 bounds를 사용하여 영역 내 랜덤 위치 계산
        Bounds bounds = collider.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    private bool IsValidPosition(Vector2 position)
    {
        foreach (Vector2 occupiedPosition in occupiedPositions)
        {
            if (Vector2.Distance(position, occupiedPosition) < minDistanceBetweenObjects)
            {
                return false; // 최소 간격을 만족하지 않으면 위치가 유효하지 않음
            }
        }
        return true; // 모든 위치가 유효하면 true 반환
    }
}
