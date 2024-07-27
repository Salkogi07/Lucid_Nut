using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suck_ob : MonoBehaviour
{
    public float moveSpeed = 0.5f;  // �÷��̾ �̵��ϴ� �ӵ�
    public float skillDuration = 5f;  // ��ų�� ���� �ð�

    private bool isUsingSkill = false;  // ��ų ��� ������ ����
    private float skillEndTime = 0f;  // ��ų�� ������ �ð�
    private GameObject player;  // "Player" �±׸� ���� ������Ʈ

    public bool SS = false;  // ��ų Ȱ��ȭ ���¸� �����ϴ� ����

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");  // "Player" �±׸� ���� ������Ʈ�� ã���ϴ�.
    }

    void Update()
    {
        // SS�� true�̰�, ��ų�� ��� ������ ���� ��
        if (SS && !isUsingSkill)
        {
            Debug.Log("���� ���̱�");
            StartSkill();
        }

        // ��ų�� ��� ���� ��
        if (isUsingSkill)
        {
            if (Time.time < skillEndTime)
            {
                MovePlayerTowardsObject();
            }
            else
            {
                EndSkill();
            }
        }
    }

    void StartSkill()
    {
        isUsingSkill = true;
        skillEndTime = Time.time + skillDuration;
        SS = false;  // ��ų ���� �� SS�� false�� ����
    }

    void MovePlayerTowardsObject()
    {
        if (player != null)
        {
            // ���� ������Ʈ(�� ��ũ��Ʈ�� �پ��ִ� ������Ʈ) ������ ����մϴ�.
            Vector3 direction = (transform.position - player.transform.position).normalized;
            // �÷��̾ ���� ������Ʈ �������� �̵���ŵ�ϴ�.
            player.transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void EndSkill()
    {
        isUsingSkill = false;
        // ��ų ���� ��, �÷��̾��� �̵��� ����ϴ�.
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}