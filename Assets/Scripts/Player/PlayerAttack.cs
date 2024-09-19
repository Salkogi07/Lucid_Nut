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
        Vector2 len = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos.transform.position;
        float z = Mathf.Atan2(len.y, len.x) * Mathf.Rad2Deg;

        pos.transform.rotation = Quaternion.Euler(0, 0, z);

        if (curTime <= 0)
        {
            // ���콺 ��Ŭ�� (�ֵθ��� ����)
            if (Input.GetMouseButtonDown(0) && !playerMove.isAttack)
            {
                float screenMiddle = Screen.width / 2;
                int direction = Input.mousePosition.x > screenMiddle ? 1 : -1;

                Debug.Log(direction);

                PerformSlashAttack(direction);  // �ֵθ��� ���� ����
                curTime = coolTime;  // ��Ÿ�� ����
            }

            // ���콺 ��Ŭ�� (��� ����)
            if (Input.GetMouseButtonDown(1) && !playerMove.isAttack)
            {
                float screenMiddle = Screen.width / 2;
                int direction = Input.mousePosition.x > screenMiddle ? 1 : -1;

                PerformStingAttack(direction);  // ��� ���� ����
                curTime = coolTime;  // ��Ÿ�� ����
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    private void PerformSlashAttack(int direction)
    {
        playerMove.isAttack = true;

        playerMove.FlipAttack(direction);  // �÷��̾��� ������ ���� �������� ����

        GameObject SE = Instantiate(SlashEffect, transform.position, pos.transform.rotation,transform);  // �ֵθ��� ����Ʈ ����
        SE.transform.rotation = direction < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        Destroy(SE, 0.3f);  // ª�� �ð� �� ����Ʈ ����

        audioSource.PlayOneShot(attackSound);  // ������ �� ���� ���

        // ������ ���� �� �� ����
        Vector2 hitBoxSize = boxSize;
        Vector2 hitBoxCenter = pos.position;  // ������ ��ġ���� ��Ʈ�ڽ� ����

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(hitBoxCenter, hitBoxSize,pos.transform.rotation.z);  // ������ ũ�� �� ���� �ݿ�
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyHP enemy = collider.GetComponent<EnemyHP>();
                if (enemy != null)
                    enemy.TakeDamage(20f);  // ������ ����

                CreateHitEffect(collider.transform.position, direction);
            }
            else if (collider.CompareTag("Boss"))
            {
                BossHp boss = collider.GetComponent<BossHp>();
                if (boss != null)
                    boss.TakeDamage(20);  // ������ ����

                CreateHitEffect(collider.transform.position, direction);
            }
        }
    }

    private void PerformStingAttack(int direction)
    {
        playerMove.isAttack = true;

        playerMove.FlipAttack(direction);  // �÷��̾��� ������ ���� �������� ����

        GameObject SE = Instantiate(SlashEffect, transform.position, pos.transform.rotation, transform);  // �ֵθ��� ����Ʈ ����
        SE.transform.rotation = direction < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        Destroy(SE, 0.3f);  // ª�� �ð� �� ����Ʈ ����

        audioSource.PlayOneShot(attackSound);  // ������ �� ���� ���

        // ��� ���� �� �� ����
        Vector2 hitBoxSize = boxSize;
        Vector2 hitBoxCenter = pos.position;  // ������ ��ġ���� ��Ʈ�ڽ� ����

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(hitBoxCenter, hitBoxSize, pos.transform.rotation.z);  // ��� ũ�� �� ���� �ݿ�
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyHP enemy = collider.GetComponent<EnemyHP>();
                if (enemy != null)
                    enemy.TakeDamage(30f);  // ������ ����

                CreateHitEffect(collider.transform.position, direction);
            }
            else if (collider.CompareTag("Boss"))
            {
                BossHp boss = collider.GetComponent<BossHp>();
                if (boss != null)
                    boss.TakeDamage(30);  // ������ ����

                CreateHitEffect(collider.transform.position, direction);
            }
        }
    }

    private void CreateHitEffect(Vector3 position, int direction)
    {
        int effectDirection = direction > 0 ? 1 : -1;
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
