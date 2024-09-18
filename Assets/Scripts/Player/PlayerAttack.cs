// PlayerAttack.cs ���� �ڵ�
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
    public GameObject AttackHitEffect;
    public GameObject SlashEffect;    // �ֵθ��� ����Ʈ
    public AudioClip attackSound;     // ���� ���� Ŭ�� �߰�
    private AudioSource audioSource;  // ����� �ҽ� �߰�

    // ���콺 �巡�� ���� �߰�
    private Vector3 startMousePosition;
    private Vector3 endMousePosition;
    private bool isDragging = false;

    public float minDragDistance = 0.5f; // �ּ� �巡�� �Ÿ�
    public float maxDragDistance = 5.0f; // �ִ� �巡�� �Ÿ�
    public float minDamage = 10f;  // �ּ� ������
    public float maxDamage = 50f;  // �ִ� ������
    public float minSlashSize = 1f; // �ּ� ������ ũ��
    public float maxSlashSize = 3f; // �ִ� ������ ũ��

    private LineRenderer lineRenderer;  // ���� ������ �߰�

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();  // PlayerMove ��ũ��Ʈ ��������
        audioSource = GetComponent<AudioSource>();  // ����� �ҽ� ������Ʈ ��������

        if (audioSource == null)
        {
            // ���� ����� �ҽ��� ������ �߰�
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // LineRenderer ����
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;  // �������� ����, �� ���� ��
        lineRenderer.startWidth = 0.1f;  // ���� ������ �β�
        lineRenderer.endWidth = 0.1f;    // ���� ���� �β�
        lineRenderer.enabled = false;    // �⺻������ ��Ȱ��ȭ
    }

    void Update()
    {
        if (curTime <= 0)
        {
            // �巡�� ���� ��� (���콺 Ŭ�� ����)
            if (Input.GetMouseButtonDown(0) && !isDragging)
            {
                startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                startMousePosition.z = 0; // 2D ȯ���̹Ƿ� z �� ����
                isDragging = true;

                // �巡�� ���� �������� ���� ������ Ȱ��ȭ
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, startMousePosition);  // ������ ����
            }

            // �巡�� ���� �� ���� ������Ʈ
            if (isDragging)
            {
                Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentMousePosition.z = 0;

                // ���콺 ���� ��ġ�� ���� ��ġ ���� �Ÿ� ���
                float dragDistance = Vector3.Distance(startMousePosition, currentMousePosition);

                // �Ÿ��� �ּ� �� �ִ� ������ ����
                dragDistance = Mathf.Clamp(dragDistance, minDragDistance, maxDragDistance);

                // ���ѵ� �Ÿ���ŭ �巡�׵� ��ġ ���
                Vector3 clampedPosition = startMousePosition + (currentMousePosition - startMousePosition).normalized * dragDistance;

                lineRenderer.SetPosition(1, clampedPosition);  // ���ѵ� �Ÿ��� ���� ���� ������Ʈ
            }

            // �巡�� ���� ���� ���� (���콺 Ŭ���� ���� ����)
            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                endMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                endMousePosition.z = 0;
                isDragging = false;

                // �巡�� �Ÿ� ���
                float dragDistance = Vector2.Distance(startMousePosition, endMousePosition);
                dragDistance = Mathf.Clamp(dragDistance, minDragDistance, maxDragDistance); // �巡�� �Ÿ� ����

                // ��ũ�� ���� �¿� ������ �������� ���� ���� ����
                Vector2 direction = (startMousePosition.x < Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0)).x) ? Vector2.left : Vector2.right;

                // ������ ���� ���� (����, ũ�� ����)
                PerformSlashAttack(dragDistance, direction);
                curTime = coolTime; // ��Ÿ�� ����

                // �巡�� �������Ƿ� ���� ������ ��Ȱ��ȭ
                lineRenderer.enabled = false;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    private void PerformSlashAttack(float dragDistance, Vector2 direction)
    {
        playerMove.isAttack = true;

        // �巡�� ���⿡ ���� ���� ����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // �巡���� ���⿡ ���� ���� ���

        Vector3 middlePoint = (startMousePosition + endMousePosition) / 2;
        Vector3 angleVec = (middlePoint - transform.position).normalized;
        float anglePos = Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg;
        Quaternion lookRotation = Quaternion.AngleAxis(anglePos, Vector3.forward);

        playerMove.FlipAttack(direction.x);  // �÷��̾��� ������ �巡���� �������� ����

        // �巡�� �Ÿ� ������� ������ �� ������ ũ�� ���
        float damage = Mathf.Lerp(maxDamage, minDamage, (dragDistance - minDragDistance) / (maxDragDistance - minDragDistance));
        float slashSize = Mathf.Lerp(minSlashSize, maxSlashSize, (dragDistance - minDragDistance) / (maxDragDistance - minDragDistance));

        GameObject SE = Instantiate(SlashEffect,pos.position,lookRotation, transform);  // �θ� ��ü�� �÷��̾� ����

        SE.transform.localScale = new Vector3(direction.x < 0 ? -slashSize : slashSize, slashSize, 1);  // ������ ũ�⸦ ������ ���̿� �°� ����

        Destroy(SE, 0.3f);  // ª�� �ð� �� ����Ʈ ����

        // ������ ���� �� �� ���� (����Ʈ ũ��� �����ϰ� ���� ����)
        Vector2 hitBoxSize = boxSize * slashSize;
        Vector2 hitBoxCenter = pos.position;  // ������ ��ġ���� ��Ʈ�ڽ� ����

        audioSource.PlayOneShot(attackSound);  // ������ �� ���� ���

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(hitBoxCenter, hitBoxSize, angle);  // ������ ũ�� �� ���� �ݿ�
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyHP enemy = collider.GetComponent<EnemyHP>();
                if (enemy != null)
                    enemy.TakeDamage(damage);  // �巡�� �Ÿ� ��� ������ ����

                float effectDirection = playerMove.isFacingRight ? 1f : -1f;
                Vector3 effectScale = new Vector3(effectDirection, 1, 1);
                GameObject AE = Instantiate(AttackHitEffect, collider.transform.position, Quaternion.identity);
                AE.transform.localScale = effectScale;
                Destroy(AE, 0.5f);
            }
            else if (collider.CompareTag("Boss"))
            {
                BossHp boss = collider.GetComponent<BossHp>();
                if (boss != null)
                    boss.TakeDamage((int)damage);  // �巡�� �Ÿ� ��� ������ ����

                float effectDirection = playerMove.isFacingRight ? 1f : -1f;
                Vector3 effectScale = new Vector3(effectDirection, 1, 1);
                GameObject AE = Instantiate(AttackHitEffect, collider.transform.position, Quaternion.identity);
                AE.transform.localScale = effectScale;
                Destroy(AE, 0.5f);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (pos == null)
            return;

        // ������ ũ�⸦ �ǽð����� ��� (���� ���� �ÿ� �����ϰ� ����)
        float dragDistance = Vector2.Distance(startMousePosition, endMousePosition);
        float slashSize = Mathf.Lerp(minSlashSize, maxSlashSize, (dragDistance - minDragDistance) / (maxDragDistance - minDragDistance));

        // OverlapBox ũ��� ��ġ
        Vector2 hitBoxSize = boxSize * slashSize;
        Vector2 hitBoxCenter = pos.position;

        // Gizmos ���� ���� (������)
        Gizmos.color = Color.red;

        // OverlapBox ������ �׸���
        Gizmos.DrawWireCube(hitBoxCenter, hitBoxSize);
    }

}
