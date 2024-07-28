using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMove2 : MonoBehaviour
{
    public float speed = 2f; // ���� �̵� �ӵ�
    public float dashSpeed = 20f; // �뽬 �ӵ�
    public float cooldownTime = 5f; // ��Ÿ�� �ð�
    private Transform player; // �÷��̾��� Ʈ������
    private Vector2 dashTarget; // �뽬 ��ǥ ��ġ
    private bool isCooldown = false; // �뽬 ��Ÿ�� ����
    private bool isDashing = false; // �뽬 �� ����
    private bool isPreparingToDash = false; // �뽬 �غ� �� ����

    void Start()
    {
        // �±װ� "Player"�� ��ü�� ã���ϴ�.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // �÷��̾� ��ü�� �����ϴ��� Ȯ���ϰ� Ʈ�������� �����մϴ�.
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        // �÷��̾ �����ϰ� �뽬 ���� �ƴϸ� �뽬 �غ� ���� �ƴ� ���� ���� �����̵��� �մϴ�.
        if (player != null && !isDashing && !isPreparingToDash)
        {
            // �÷��̾�� �� ������ ������ ����մϴ�.
            Vector2 direction = player.position - transform.position;
            direction.Normalize();

            // ���� �÷��̾� �������� �̵���ŵ�ϴ�.
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �ݶ��̴� �ȿ� �±װ� "Player"�� ��ü�� ���Դ��� Ȯ���մϴ�.
        if (other.CompareTag("Player") && !isCooldown && !isPreparingToDash)
        {
            StartCoroutine(PrepareAndDash(other.transform));
        }
    }

    private IEnumerator PrepareAndDash(Transform playerTransform)
    {
        // �뽬 �غ� ������ ����
        isPreparingToDash = true;

        // 0.5�� ���� ����ϴ�.
        yield return new WaitForSeconds(0.5f);

        // �÷��̾��� ���� ��ġ�� �����մϴ�.
        dashTarget = playerTransform.position;

        // �뽬�ϴ� ���� �뽬 ������ �����մϴ�.
        isDashing = true;
        isPreparingToDash = false;

        // �뽬�ϴ� ���� �÷��̾� ��ġ�� �̵��մϴ�.
        while (Vector2.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null;
        }

        // �뽬�� ���� �� ��Ÿ���� �����մϴ�.
        isCooldown = true;
        isDashing = false;
        yield return new WaitForSeconds(cooldownTime);

        // ��Ÿ�� ����
        isCooldown = false;
    }
}