using UnityEngine;

public class RainEnemy : MonoBehaviour
{
    public GameObject rainPrefab; // ���� ������Ʈ�� ������
    public float cooldownTime = 5f; // ��Ÿ�� (5��)
    public float rainSpeed = 10f; // ������ �ӵ�
    public int numberOfRains = 10; // �߻��� ������ ��

    private float lastShootTime = 0f; // ������ �߻� �ð�
    private Transform playerTransform; // �÷��̾��� Transform

    void Start()
    {
        // �±װ� "Player"�� ������Ʈ�� ã���ϴ�.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // ���� �ð��� ������ �߻� �ð��� ���̸� Ȯ���Ͽ� ��Ÿ���� �������� üũ
        if (Time.time - lastShootTime >= cooldownTime)
        {
            // �÷��̾ �����ϴ��� Ȯ��
            if (playerTransform != null)
            {
                ShootRain();
                lastShootTime = Time.time; // �߻� �ð� ������Ʈ
            }
        }
    }

    void ShootRain()
    {
        if (rainPrefab != null)
        {
            Vector3 shooterPosition = transform.position;

            for (int i = 0; i < numberOfRains; i++)
            {
                // ���� ������Ʈ ����
                GameObject rain = Instantiate(rainPrefab, shooterPosition, Quaternion.identity);

                // ���� ������Ʈ�� ������ �÷��̾� �������� ����
                Vector3 direction = (playerTransform.position - shooterPosition).normalized;
                rain.GetComponent<Rigidbody2D>().velocity = direction * rainSpeed;
            }
        }
    }
}
