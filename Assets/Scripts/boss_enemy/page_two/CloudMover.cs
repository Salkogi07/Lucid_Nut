using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudMover : MonoBehaviour
{
    public float speed = 5.0f; // 이동 속도
    public GameObject attackPointPrefab; // 공격 포인트에 추가할 오브젝트 프리팹
    public GameObject temporaryObjectPrefab; // 1초 동안 두는 임시 오브젝트 프리팹
    public float initialDelay = 3.0f; // 초기 공격 포인트 설정 전 대기 시간
    public float waitTimeAtAttackPoint = 6.0f; // 공격 포인트 도착 후 대기 시간
    public float temporaryObjectDuration = 1.0f; // 임시 오브젝트를 유지할 시간
    public float objectLifetime = 20.0f; // 오브젝트의 생명 시간

    private Transform pointA;
    private Transform pointB;
    private Transform targetPoint;
    private Vector3 attackPoint;
    private GameObject currentAttackPointObject; // 현재 공격 포인트 오브젝트

    private List<GameObject> instantiatedPrefabs = new List<GameObject>(); // 인스턴스화된 프리팹을 추적하는 리스트

    void Start()
    {
        // 태그가 'cloud_move_point'인 오브젝트 찾기
        GameObject[] cloudPoints = GameObject.FindGameObjectsWithTag("cloud_move_point");

        // 두 개의 오브젝트만 찾아야 하므로 확인
        if (cloudPoints.Length != 2)
        {
            Debug.LogError("There should be exactly two objects with the tag 'cloud_move_point'");
            return;
        }

        // 각각의 오브젝트를 pointA와 pointB에 할당
        pointA = cloudPoints[0].transform;
        pointB = cloudPoints[1].transform;

        // 초기 목표 지점을 pointA로 설정
        targetPoint = pointA;

        // 3초 후에 초기 공격 포인트 설정
        Invoke("InitializeAttackPoint", initialDelay);

        // 20초 후에 이 오브젝트를 파괴
        Invoke("DestroyAll", objectLifetime);
    }

    void Update()
    {
        if (pointA == null || pointB == null)
            return;

        // 현재 위치에서 목표 지점까지 이동
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, step);

        // 목표 지점에 도착했을 때의 처리
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // 목표 지점을 변경
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트의 태그가 'abc'인지 확인
        if (other.CompareTag("abc"))
        {
            if (currentAttackPointObject != null)
            {
                // 현재 공격 포인트 오브젝트 삭제 전, 임시 오브젝트 생성
                Vector3 position = currentAttackPointObject.transform.position;
                Destroy(currentAttackPointObject);

                // 임시 오브젝트를 생성하여 1초 동안 유지
                StartCoroutine(CreateAndDestroyTemporaryObject(position));
            }

            Debug.Log("공격지점도달");
            // 6초 대기 후 공격 포인트를 설정하도록 코루틴 시작
            StartCoroutine(WaitAndSetRandomAttackPoint());
        }
    }

    IEnumerator WaitAndSetRandomAttackPoint()
    {
        // 6초 대기
        yield return new WaitForSeconds(waitTimeAtAttackPoint);

        // 랜덤 공격 포인트 설정
        SetRandomAttackPoint();
    }

    IEnumerator CreateAndDestroyTemporaryObject(Vector3 position)
    {
        // 임시 오브젝트를 생성
        if (temporaryObjectPrefab != null)
        {
            GameObject temporaryObject = Instantiate(temporaryObjectPrefab, position, Quaternion.identity);
            instantiatedPrefabs.Add(temporaryObject); // 리스트에 추가
            // 지정된 시간 동안 유지
            yield return new WaitForSeconds(temporaryObjectDuration);
            Destroy(temporaryObject);
            instantiatedPrefabs.Remove(temporaryObject); // 리스트에서 제거
        }
    }

    void InitializeAttackPoint()
    {
        // 초기 공격 포인트 설정
        SetRandomAttackPoint();
    }

    void SetRandomAttackPoint()
    {
        // pointA와 pointB 사이의 랜덤 위치를 생성
        float t = Random.value;
        attackPoint = Vector3.Lerp(pointA.position, pointB.position, t);

        // 새로운 공격 포인트 오브젝트를 생성
        if (attackPointPrefab != null)
        {
            currentAttackPointObject = Instantiate(attackPointPrefab, attackPoint, Quaternion.identity);
            instantiatedPrefabs.Add(currentAttackPointObject); // 리스트에 추가
        }
    }

    void DestroyAll()
    {
        // 인스턴스화된 모든 프리팹을 파괴
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            if (prefab != null)
            {
                Destroy(prefab);
            }
        }

        // 이 오브젝트를 파괴
        Destroy(gameObject);
    }
}
