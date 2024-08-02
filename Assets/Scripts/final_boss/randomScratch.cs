using UnityEngine;
using System.Collections;

public class RandomObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // 소환할 오브젝트
    public float spawnInterval = 0.05f; // 소환 간격
    public float spawnDuration = 10f; // 소환 지속 시간
    public Vector2 spawnAreaMin; // 소환 영역의 최소 좌표
    public Vector2 spawnAreaMax; // 소환 영역의 최대 좌표

    public bool RandScratch = false; // 소환 활성화 여부를 결정하는 변수

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        float elapsedTime = 0f;

        while (elapsedTime < spawnDuration)
        {
            if (RandScratch)
            {
                SpawnRandomObject();
                yield return new WaitForSeconds(spawnInterval);
                elapsedTime += spawnInterval;
            }
            else
            {
                yield return null; // RandScratch가 false일 경우 다음 프레임까지 기다림
            }
        }

        RandScratch = false; // 소환 기간이 끝나면 RandScratch를 false로 설정
    }

    void SpawnRandomObject()
    {
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}
