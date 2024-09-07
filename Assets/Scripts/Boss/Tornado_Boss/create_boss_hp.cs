using UnityEngine;

public class create_boss_hp : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트의 프리팹
    public Collider2D spawnAreaCollider; // 오브젝트가 생성될 콜라이더

    private GameObject currentObject;
    private bool isSpawning = false; // 현재 스폰 대기 중인지 추적하는 플래그

    void Start()
    {
        SpawnObject();
    }

    void SpawnObject()
    {
        // 콜라이더 안에서 랜덤한 위치 생성
        Vector2 randomPosition = GetRandomPositionInCollider();

        // 오브젝트 생성
        currentObject = Instantiate(objectPrefab, randomPosition, Quaternion.identity);
        isSpawning = false; // 오브젝트 생성 완료 후 스폰 대기 상태 해제
    }

    Vector2 GetRandomPositionInCollider()
    {
        // 콜라이더의 바운딩 박스 가져오기
        Bounds bounds = spawnAreaCollider.bounds;

        // 바운스 안에서 랜덤한 위치 선택
        Vector2 randomPosition = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );

        // 랜덤 위치를 콜라이더의 내부로 제한
        Vector2 closestPoint = spawnAreaCollider.ClosestPoint(randomPosition);
        if ((Vector2)randomPosition != closestPoint)
        {
            randomPosition = closestPoint;
        }

        return randomPosition;
    }

    private void Update()
    {
        if (currentObject == null && !isSpawning)
        {
            isSpawning = true; // 스폰 대기 상태 설정
            Invoke("SpawnObject", 2f); // 2초 후에 오브젝트를 생성
        }
    }
}
