using UnityEngine;

public class moveT : MonoBehaviour
{
    public float speed = 5f; // �̵� �ӵ�
    private bool movingLeft = true; // �ʱ� ������ �������� ����

    void Update()
    {
        // ���� ���⿡ ���� �̵�
        if (movingLeft)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ������Ʈ�� �±װ� "wall"���� Ȯ��
        if (collision.gameObject.CompareTag("wall"))
        {
            Debug.Log("������ȯ"); // �浹 ���� Ȯ�ο� �α�
            // ���� ��ȯ
            movingLeft = !movingLeft;
        }
    }

    // ����׿�: �ݶ��̴� ������ �ð������� ǥ��
    void OnDrawGizmos()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, collider.bounds.size);
        }
    }
}
