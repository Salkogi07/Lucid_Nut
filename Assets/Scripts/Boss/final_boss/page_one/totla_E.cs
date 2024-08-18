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
    private float speedIncreaseFactor = 2f; // 속도를 증가시키는 계수

    private Vector3 moveDirection; // 이동 방향

    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        GameObject[] totalObjects = GameObject.FindGameObjectsWithTag("total_E");
        if (totalObjects.Length > 0)
        {
            targetObject = totalObjects[0];
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("energe"))
        {
            Debug.Log("ene");
            Destroy(other.gameObject);
            energeCount++;

            Debug.Log(energeCount);
            if (energeCount >= 100 && !hasStartedMoving)
            {
                hasStartedMoving = true;
                StartCoroutine(DetermineMoveDirection());
            }
        }
    }

    void Update()
    {
        if (!hasStartedMoving && targetObject != null)
        {
            Vector3 newScale = Vector3.one * (energeCount * 0.15f);
            targetObject.transform.localScale = newScale;
        }

        if (hasStartedMoving && targetObject != null)
        {
            // 이동 방향으로 계속 이동
            targetObject.transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator DetermineMoveDirection()
    {
        // 2초 대기
        yield return new WaitForSeconds(3f);

        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            // 플레이어의 위치로 이동 방향 설정
            Vector3 targetPosition = playerObject.transform.position;
            moveDirection = (targetPosition - targetObject.transform.position).normalized;
            moveSpeed *= speedIncreaseFactor; // 이동 속도 증가
        }
        else
        {
            Debug.LogWarning("No object with tag 'Player' found.");
        }

        // 이동이 시작되면 게임 오브젝트를 삭제하지 않음
        // Destroy(gameObject);
    }
}
