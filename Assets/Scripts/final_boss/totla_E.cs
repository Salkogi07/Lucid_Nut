using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class total_E : MonoBehaviour
{
    private int energeCount = 0; // 충돌한 energe 태그를 가진 오브젝트의 개수
    private GameObject targetObject; // 크기를 조절할 대상 오브젝트
    private GameObject playerObject; // Player 오브젝트를 저장할 변수
    private bool hasStartedMoving = false; // 이동을 시작했는지 여부

    private float moveSpeed = 5f; // 목표 오브젝트를 이동시키는 속도
    private float moveDuration = 2f; // 플레이어를 향해 이동할 시간
    private float speedIncreaseFactor = 2f; // 속도를 증가시키는 계수

    private Vector3 moveDirection; // 이동 방향

    void Start()
    {
        // Collider2D의 "Is Trigger" 체크를 활성화하여 트리거 모드로 설정합니다.
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        // 태그가 "total_E"인 오브젝트를 찾습니다.
        GameObject[] totalObjects = GameObject.FindGameObjectsWithTag("total_E");
        if (totalObjects.Length > 0)
        {
            targetObject = totalObjects[0]; // 첫 번째 오브젝트를 선택
            Debug.Log("Target object found: " + targetObject.name);
        }
        else
        {
            Debug.LogWarning("No object with tag 'total_E' found.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("energe"))
        {
            Debug.Log("ene");

            // energe 태그를 가진 오브젝트를 삭제합니다.
            Destroy(other.gameObject);

            // 충돌한 energe 오브젝트의 개수를 증가시킵니다.
            energeCount++;

            Debug.Log(energeCount);
            // energeCount가 100이 되면 Player 오브젝트를 찾아서 이동 시작
            if (energeCount >= 100 && !hasStartedMoving)
            {
                hasStartedMoving = true;
                StartCoroutine(MoveToPlayer());
            }
        }
    }

    void Update()
    {
        if (!hasStartedMoving && targetObject != null)
        {
            // 크기 알고리즘: 닿은 오브젝트 개수 * 0.3
            Vector3 newScale = Vector3.one * (energeCount * 0.15f);
            targetObject.transform.localScale = newScale;
        }

        // 이동 방향으로 계속 이동
        if (hasStartedMoving)
        {
            if (targetObject != null)
            {
                // 계속해서 이동 방향으로 이동합니다.
                targetObject.transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
    }

    IEnumerator MoveToPlayer()
    {
        // 2초 대기
        yield return new WaitForSeconds(3f);

        // Player 태그를 가진 오브젝트를 찾습니다.
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Vector3 startPosition = targetObject.transform.position;
            Vector3 targetPosition = playerObject.transform.position;

            float elapsedTime = 0f;
            float totalDistance = Vector3.Distance(startPosition, targetPosition);

            while (elapsedTime < moveDuration)
            {
                if (targetObject != null)
                {
                    float distanceCovered = (elapsedTime / moveDuration) * totalDistance;
                    float speed = Mathf.Lerp(moveSpeed, moveSpeed * speedIncreaseFactor, elapsedTime / moveDuration);
                    float step = speed * Time.deltaTime;

                    targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetPosition, step);

                    // 이동 방향을 계산
                    moveDirection = (targetPosition - targetObject.transform.position).normalized;
                }

                elapsedTime += Time.deltaTime;
                yield return null; // 매 프레임 대기
            }

            // 이동이 완료된 후에도 계속 이동하도록 방향을 설정
            if (targetObject != null)
            {
                moveDirection = (playerObject.transform.position - targetObject.transform.position).normalized;
            }
        }
        else
        {
            Debug.LogWarning("No object with tag 'Player' found.");
        }

        // collider 오브젝트를 삭제합니다.
        Destroy(gameObject);
    }
}
