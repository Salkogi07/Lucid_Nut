using UnityEngine;
using System.Collections;

public class RandomRockSpawner : MonoBehaviour
{
    public GameObject rockPrefab; // 돌 프리팹을 할당합니다.
    public int numberOfRocksPerInterval = 10; // 10초마다 생성할 돌의 개수
    public float spawnInterval = 10f; // 생성 간격(초)

    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        StartCoroutine(SpawnRocksRoutine());
    }

    IEnumerator SpawnRocksRoutine()
    {
        while (true)
        {
            for (int i = 0; i < numberOfRocksPerInterval; i++)
            {
                Vector3 randomPosition = GetRandomPositionInBox();
                Instantiate(rockPrefab, randomPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomPositionInBox()
    {
        Vector3 center = transform.position + boxCollider.center;
        Vector3 size = boxCollider.size;

        float randomX = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float randomY = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float randomZ = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(randomX, randomY, randomZ);
    }
}