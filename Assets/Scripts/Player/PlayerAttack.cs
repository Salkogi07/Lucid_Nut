using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerMove playerMove;  // PlayerMove ��ũ��Ʈ�� ����

    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;  // ����Ʈ�� ������ ���� ��ġ
    public Vector2 boxSize;
    public GameObject SlashEffect;    // �ֵθ��� ����Ʈ
    public GameObject StingEffect;    // ��� ����Ʈ

    public GameObject AttackHitEffect;
    public AudioClip attackSound;     // ���� ���� Ŭ�� �߰�
    private AudioSource audioSource;  // ����� �ҽ� �߰�

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();  // PlayerMove ��ũ��Ʈ ��������
        audioSource = GetComponent<AudioSource>();  // ����� �ҽ� ������Ʈ ��������

        if (audioSource == null)
        {
            // ���� ����� �ҽ��� ������ �߰�
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (curTime <= 0)
        {
            // ���콺 ��Ŭ�� (�ֵθ��� ����)
            if (Input.GetMouseButtonDown(0) && !playerMove.isAttack)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector2 direction = (mousePosition.x < transform.position.x) ? Vector2.left : Vector2.right;

                PerformSlashAttack(direction, mousePosition);  // �ֵθ��� ���� ����
                curTime = coolTime;  // ��Ÿ�� ����
            }

            // ���콺 ��Ŭ�� (��� ����)
            if (Input.GetMouseButtonDown(1) && !playerMove.isAttack)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector2 direction = (mousePosition.x < transform.position.x) ? Vector2.left : Vector2.right;

                PerformStingAttack(direction, mousePosition);  // ��� ���� ����
                curTime = coolTime;  // ��Ÿ�� ����
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    private void PerformSlashAttack(Vector2 direction, Vector2 mousePos)
    {
        playerMove.isAttack = true;

        Vector2 len = mousePos - new Vector2(pos.transform.position.x, pos.transform.position.y);
        float angle = Mathf.Atan2(len.y, len.x) * Mathf.Rad2Deg;

        Quaternion lookRotation = Quaternion.Euler(0, 0, angle);

        playerMove.FlipAttack(direction.x);  // �÷��̾��� ������ ���� �������� ����

        GameObject SE = Instantiate(SlashEffect, pos.position, lookRotation, transform);  // �ֵθ��� ����Ʈ ����
        SE.transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
        Destroy(SE, 0.3f);  // ª�� �ð� �� ����Ʈ ����

        audioSource.PlayOneShot(attackSound);  // ������ �� ���� ���

        // ������ ���� �� �� ����
        Vector2 hitBoxSize = boxSize;
        Vector2 hitBoxCenter = pos.position;  // ������ ��ġ���� ��Ʈ�ڽ� ����

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(hitBoxCenter, hitBoxSize, angle);  // ������ ũ�� �� ���� �ݿ�
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyHP enemy = collider.GetComponent<EnemyHP>();
                if (enemy != null)
                    enemy.TakeDamage(20f);  // ������ ����

                CreateHitEffect(collider.transform.position, direction.x);
            }
            else if (collider.CompareTag("Boss"))
            {
                BossHp boss = collider.GetComponent<BossHp>();
                if (boss != null)
                    boss.TakeDamage(20);  // ������ ����

                CreateHitEffect(collider.transform.position, direction.x);
            }
        }
    }

    private void PerformStingAttack(Vector2 direction, Vector2 mousePos)
    {
        playerMove.isAttack = true;

        Vector2 len = mousePos - new Vector2(pos.transform.position.x, pos.transform.position.y);
        float angle = Mathf.Atan2(len.y, len.x) * Mathf.Rad2Deg;

        Quaternion lookRotation = Quaternion.Euler(0, 0, angle);

        playerMove.FlipAttack(direction.x);  // �÷��̾��� ������ ���� �������� ����

        GameObject SE = Instantiate(StingEffect, pos.position, lookRotation, transform);  // ��� ����Ʈ ����
        SE.transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
        Destroy(SE, 0.3f);  // ª�� �ð� �� ����Ʈ ����

        audioSource.PlayOneShot(attackSound);  // ������ �� ���� ���

        // ��� ���� �� �� ����
        Vector2 hitBoxSize = boxSize;
        Vector2 hitBoxCenter = pos.position;  // ������ ��ġ���� ��Ʈ�ڽ� ����

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(hitBoxCenter, hitBoxSize, angle);  // ��� ũ�� �� ���� �ݿ�
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyHP enemy = collider.GetComponent<EnemyHP>();
                if (enemy != null)
                    enemy.TakeDamage(30f);  // ������ ����

                CreateHitEffect(collider.transform.position, direction.x);
            }
            else if (collider.CompareTag("Boss"))
            {
                BossHp boss = collider.GetComponent<BossHp>();
                if (boss != null)
                    boss.TakeDamage(30);  // ������ ����

                CreateHitEffect(collider.transform.position, direction.x);
            }
        }
    }

    private void CreateHitEffect(Vector3 position, float direction)
    {
        float effectDirection = direction > 0 ? 1f : -1f;
        Vector3 effectScale = new Vector3(effectDirection, 1, 1);

        GameObject AE = Instantiate(AttackHitEffect, position, Quaternion.identity);
        AE.transform.localScale = effectScale;
        Destroy(AE, 0.5f);
    }

    private void OnDrawGizmos()
    {
        if (pos == null)
            return;

        // OverlapBox ũ��� ��ġ
        Vector2 hitBoxSize = boxSize;
        Vector2 hitBoxCenter = pos.position;

        // Gizmos ���� ���� (������)
        Gizmos.color = Color.red;

        // OverlapBox ������ �׸���
        Gizmos.DrawWireCube(hitBoxCenter, hitBoxSize);
    }
}
