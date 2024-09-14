using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMovement : MonoBehaviour
{
    public float speed = 5f;
    public float pullSpeed = 10f;
    public int damageAmount = 10;
    public float pullOffsetY = 1f; // �÷��̾ ���� ������ ������ Y��
    private PlayerHp player;
    private bool pullingPlayer = false;
    public GameObject wallcrashEffect;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerHp>();
    }

    void Start()
    {
        // 2�� ��� �Ŀ� ������ ����
        StartCoroutine(StartMovementAfterDelay(2f));
    }

    IEnumerator StartMovementAfterDelay(float delay)
    {
        // delay �ð���ŭ ��ٸ�
        yield return new WaitForSeconds(delay);
        // 2�ʰ� ���� �Ŀ� Update���� �̵� ����
        while (true)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player ������Ʈ�� ���� ������Ʈ�� ���Ƶ���
            pullingPlayer = true;
            Vector3 direction = transform.position - other.transform.position;

            // ��ǥ ��ġ�� Y���� �����ؼ� �������� ������
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + pullOffsetY, transform.position.z);

            other.transform.position = Vector3.MoveTowards(other.transform.position, targetPosition, pullSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pullingPlayer = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tornado_Wall") && pullingPlayer)
        {
            GameObject effect = Instantiate(wallcrashEffect,transform.position, Quaternion.identity);
            effect.GetComponent<PlayerDamageCol>().attackDamage = damageAmount;
            Destroy(effect,0.5f);
            Destroy(gameObject);
        }
    }
}
