using UnityEngine;
using System.Collections;

public class RandomRockSpawner : MonoBehaviour
{
    public GameObject rockPrefab; // 돌 프리팹을 할당합니다.
    public int numberOfRocksPerInterval = 10; // 10초마다 생성할 돌의 개수
    public float spawnInterval = 10f; // 생성 간격(초)
    public GameObject suckRockObject; // suck_rock 스크립트가 붙어있는 오브젝트를 할당합니다.
    public GameObject colliderObject; // 콜라이더를 사용할 오브젝트를 할당합니다.
    public bool SRR;
    private BoxCollider boxCollider;
    private suck_rock suckRockScript;

    void Start()
    {
        SRR = false;
        if (colliderObject != null)
        {
            boxCollider = colliderObject.GetComponent<BoxCollider>();
        }
        if (suckRockObject != null)
        {
            suckRockScript = suckRockObject.GetComponent<suck_rock>();
        }
    }
    public void Update()
    {
        if (SRR)
        {
            SpawnRocksRoutine();
            SRR = false;
        }
    }
    public void SpawnRocksRoutine()
    {
        while (SRR)
        {
            for (int i = 0; i < numberOfRocksPerInterval; i++)
            {
                Vector3 randomPosition = GetRandomPositionInBox();
                Instantiate(rockPrefab, randomPosition, Quaternion.identity);
            }

            // suck_rock 스크립트의 SR 변수를 true로 설정합니다.
            if (suckRockScript != null)
            {
                suckRockScript.SR = true;
            }

            break;
        }
    }

    Vector3 GetRandomPositionInBox()
    {
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider is not assigned. Please assign a BoxCollider to the colliderObject.");
            return Vector3.zero;
        }

        Vector3 center = colliderObject.transform.position + boxCollider.center;
        Vector3 size = boxCollider.size;

        float randomX = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float randomY = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float randomZ = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(randomX, randomY, randomZ);
    }
}
