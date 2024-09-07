using UnityEngine;

public class create_boss_hp : MonoBehaviour
{
    public GameObject objectPrefab; // ������ ������Ʈ�� ������
    public Collider2D spawnAreaCollider; // ������Ʈ�� ������ �ݶ��̴�

    private GameObject currentObject;
    private bool isSpawning = false; // ���� ���� ��� ������ �����ϴ� �÷���

    void Start()
    {
        SpawnObject();
    }

    void SpawnObject()
    {
        // �ݶ��̴� �ȿ��� ������ ��ġ ����
        Vector2 randomPosition = GetRandomPositionInCollider();

        // ������Ʈ ����
        currentObject = Instantiate(objectPrefab, randomPosition, Quaternion.identity);
        isSpawning = false; // ������Ʈ ���� �Ϸ� �� ���� ��� ���� ����
    }

    Vector2 GetRandomPositionInCollider()
    {
        // �ݶ��̴��� �ٿ�� �ڽ� ��������
        Bounds bounds = spawnAreaCollider.bounds;

        // �ٿ �ȿ��� ������ ��ġ ����
        Vector2 randomPosition = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );

        // ���� ��ġ�� �ݶ��̴��� ���η� ����
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
            isSpawning = true; // ���� ��� ���� ����
            Invoke("SpawnObject", 2f); // 2�� �Ŀ� ������Ʈ�� ����
        }
    }
}
