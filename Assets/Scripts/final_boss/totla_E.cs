using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class total_E : MonoBehaviour
{
    private int energeCount = 0; // �浹�� energe �±׸� ���� ������Ʈ�� ����
    private GameObject targetObject; // ũ�⸦ ������ ��� ������Ʈ
    private GameObject playerObject; // Player ������Ʈ�� ������ ����
    private bool hasStartedMoving = false; // �̵��� �����ߴ��� ����

    private float moveSpeed = 5f; // ��ǥ ������Ʈ�� �̵���Ű�� �ӵ�
    private float moveDuration = 2f; // �÷��̾ ���� �̵��� �ð�
    private float speedIncreaseFactor = 2f; // �ӵ��� ������Ű�� ���

    private Vector3 moveDirection; // �̵� ����

    void Start()
    {
        // Collider2D�� "Is Trigger" üũ�� Ȱ��ȭ�Ͽ� Ʈ���� ���� �����մϴ�.
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        // �±װ� "total_E"�� ������Ʈ�� ã���ϴ�.
        GameObject[] totalObjects = GameObject.FindGameObjectsWithTag("total_E");
        if (totalObjects.Length > 0)
        {
            targetObject = totalObjects[0]; // ù ��° ������Ʈ�� ����
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

            // energe �±׸� ���� ������Ʈ�� �����մϴ�.
            Destroy(other.gameObject);

            // �浹�� energe ������Ʈ�� ������ ������ŵ�ϴ�.
            energeCount++;

            Debug.Log(energeCount);
            // energeCount�� 100�� �Ǹ� Player ������Ʈ�� ã�Ƽ� �̵� ����
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
            // ũ�� �˰���: ���� ������Ʈ ���� * 0.3
            Vector3 newScale = Vector3.one * (energeCount * 0.15f);
            targetObject.transform.localScale = newScale;
        }

        // �̵� �������� ��� �̵�
        if (hasStartedMoving)
        {
            if (targetObject != null)
            {
                // ����ؼ� �̵� �������� �̵��մϴ�.
                targetObject.transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
    }

    IEnumerator MoveToPlayer()
    {
        // 2�� ���
        yield return new WaitForSeconds(3f);

        // Player �±׸� ���� ������Ʈ�� ã���ϴ�.
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

                    // �̵� ������ ���
                    moveDirection = (targetPosition - targetObject.transform.position).normalized;
                }

                elapsedTime += Time.deltaTime;
                yield return null; // �� ������ ���
            }

            // �̵��� �Ϸ�� �Ŀ��� ��� �̵��ϵ��� ������ ����
            if (targetObject != null)
            {
                moveDirection = (playerObject.transform.position - targetObject.transform.position).normalized;
            }
        }
        else
        {
            Debug.LogWarning("No object with tag 'Player' found.");
        }

        // collider ������Ʈ�� �����մϴ�.
        Destroy(gameObject);
    }
}
