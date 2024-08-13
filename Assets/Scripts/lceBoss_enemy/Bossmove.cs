using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossmove : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform�� �����մϴ�.
    public float speed = 5f; // ���� �̵��ϴ� �ӵ��Դϴ�.
    public float stoppingDistance = 1f; // �÷��̾���� ���ߴ� �Ÿ��Դϴ�.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    void Update()
    {
        // �÷��̾���� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, player.position);

        // �÷��̾ Ư�� �Ÿ����� �ָ� ���� �� ����
        if (distance > stoppingDistance)
        {
            // �÷��̾� �������� �̵�
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // �÷��̾ �ٶ󺸵��� ȸ��
            transform.LookAt(player);
        }
    }
}
