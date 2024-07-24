using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suck_ob : MonoBehaviour
{
    public float moveSpeed = 0.5f;  // �÷��̾ �̵��ϴ� �ӵ�
    public float skillDuration = 5f;  // ��ų�� ���� �ð�
    public float cooldownTime = 10f;  // ��Ÿ�� �ð�

    private bool isUsingSkill = false;  // ��ų ��� ������ ����
    private float skillEndTime = 0f;  // ��ų�� ������ �ð�
    private float nextSkillTime = 0f;  // ���� ��ų ��� ���� �ð�
    private GameObject player;  // "Player" �±׸� ���� ������Ʈ

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");  // "Player" �±׸� ���� ������Ʈ�� ã���ϴ�.
    }

    void Update()
    {
        // ���� �ð��� ���� ��ų ��� ���� �ð����� ũ��, ��ų�� ��� ������ ���� ��
        if (Time.time >= nextSkillTime && !isUsingSkill)
        {
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
        nextSkillTime = Time.time + cooldownTime;
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
