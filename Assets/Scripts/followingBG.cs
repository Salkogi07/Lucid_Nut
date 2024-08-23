using UnityEngine;

public class followingBG : MonoBehaviour
{
    public float smoothSpeed = 0.125f; // ����� ������� �ӵ�

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPosition = player.transform.position;
        Vector2 backgroundPosition = this.transform.position;

        // Time.deltaTime�� ����� �����ӿ� �������� ���� ���
        backgroundPosition.x = Mathf.Lerp(backgroundPosition.x, playerPosition.x, smoothSpeed * Time.deltaTime);

        // ����� ��ġ�� ������Ʈ
        this.transform.position = backgroundPosition;
    }
}
