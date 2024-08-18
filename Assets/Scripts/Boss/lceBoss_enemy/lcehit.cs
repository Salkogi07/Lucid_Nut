/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lcehit : MonoBehaviour
{
    public GameObject hitPrefab;
    public float hit;
    public float cooldownTime = 1f;
    private float lastShootTime = 0f;

    private Transform playerTransform;


    private GameObject[] rainObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastShootTime >= cooldownTime)
        {
            // 플레이어가 존재하는지 확인
            if (playerTransform != null)
            {
                hiton();
                lastShootTime = Time.time; // 발사 시간 업데이트
            }
        }

        void hiton()
        {

            {
                Vector3 shooterPosition = transform.position;


                hitObjects = new GameObject[numberOfRains];
                for (int i = 0; i < numberOfRains; i++)
                {
                    // 겹치지 않게 가로로 오프셋을 적용하여 빗물 오브젝트 위치 설정
                    Vector3 spawnPosition = shooterPosition +  Vector3(Random.Range(-10.0f, 10.0f), 10, 0);

                    // 빗물 오브젝트 생성
                    rainObjects[i] = Instantiate(rainPrefab, spawnPosition, Quaternion.identity);
                }

                // 발사 전 대기 시간 후 발사 시작
                StartCoroutine(FireRainsAfterDelay());
            }
        }
    }
}*/
