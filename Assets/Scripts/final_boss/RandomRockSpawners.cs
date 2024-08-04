using UnityEngine;
using System.Collections;

public class RandomRockSpawners : MonoBehaviour
{
    public GameObject rockPrefab; // 돌 프리팹을 할당합니다.
    public int numberOfRocksPerInterval = 10; // 10초마다 생성할 돌의 개수
    public float spawnInterval = 10f; // 생성 간격(초)
    public GameObject suckRockObject; // suck_rock2 스크립트가 붙어있는 오브젝트를 할당합니다.
    public GameObject[] colliderObjects; // 여러 개의 콜라이더를 사용할 오브젝트를 할당합니다.
    public bool SRR;

    private suck_rock2 suckRockScript;

    void Start()
    {
        SRR = false;

        if (colliderObjects.Length > 0)
        {
            // 기본적으로 첫 번째 콜라이더를 선택
            // Collider 초기화는 Start()에서 하며, 고정된 콜라이더 인덱스를 사용하지 않습니다.
        }
        if (suckRockObject != null)
        {
            suckRockScript = suckRockObject.GetComponent<suck_rock2>();
        }
    }

    void Update()
    {
        if (SRR)
        {
            StartCoroutine(SpawnRocksRoutine()); // 코루틴 사용
            SRR = false;
        }
    }

    IEnumerator SpawnRocksRoutine()
    {
        while (SRR)
        {
            // 각 콜라이더에서 돌을 생성하도록 설정
            int rocksPerCollider = Mathf.CeilToInt((float)numberOfRocksPerInterval / colliderObjects.Length);

            for (int i = 0; i < colliderObjects.Length; i++)
            {
                GameObject currentColliderObject = colliderObjects[i];
                BoxCollider currentBoxCollider = currentColliderObject.GetComponent<BoxCollider>();

                for (int j = 0; j < rocksPerCollider; j++)
                {
                    Vector3 randomPosition = GetRandomPositionInBox(currentBoxCollider);
                    Instantiate(rockPrefab, randomPosition, Quaternion.identity);
                }
            }

            // suck_rock 스크립트의 SR 변수를 true로 설정합니다.
            if (suckRockScript != null)
            {
                suckRockScript.SR = true;
            }

            yield return new WaitForSeconds(spawnInterval); // 생성 간격만큼 대기
        }
    }

    Vector3 GetRandomPositionInBox(BoxCollider boxCollider)
    {
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider is not assigned.");
            return Vector3.zero;
        }

        Vector3 center = boxCollider.transform.position + boxCollider.center;
        Vector3 size = boxCollider.size;

        float randomX = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float randomY = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float randomZ = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(randomX, randomY, randomZ);
    }
}
