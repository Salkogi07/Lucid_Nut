using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DC_Sc : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer; // SpriteRenderer �߰�

    [Header("Move info")]
    private Rigidbody2D rigid;
    public bool move = true;
    public float moveSpeed = 3f;
    public int nextmove;

    [Header("Dash info")]
    public bool isDashing = false; // �뽬 �� ����
    public bool isPreparingToDash = false; // �뽬 �غ� �� ����
    private Vector2 dashTarget;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float cooldownTime = 0.7f;
    public Vector2 detectionSize1 = new Vector2(5f, 2f); // �÷��̾� ���� ���� (�簢�� ũ��)
    public Vector2 detectionOffset1 = Vector2.zero; // �簢���� ��ġ ������
    public LayerMask playerLayer1; // �÷��̾� ���̾�
    public GameObject dashEffect;

    [Header("Detection info")]
    public Vector2 detectionSize = new Vector2(15f, 5f); // �÷��̾� ���� ���� (�簢�� ũ��)
    public Vector2 detectionOffset = Vector2.zero; // �簢���� ��ġ ������
    public LayerMask playerLayer; // �÷��̾� ���̾�
    public bool nmove = false;

    private Transform playerTransform;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
        think();
    }

    void FixedUpdate()
    {
        if (move && !isDashing) // �뽬 ���� �ƴ� ���� �̵�
        {
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);
        }
    }

    private void Update()
    {
        spriteRenderer.flipX = rigid.velocity.x > 0;
        DetectAndMoveTowardsPlayer();
        Dashcoll();
    }

    void think()
    {
        if (!nmove)
        {
            nextmove = Random.Range(-1, 2);
        }
        float nextTimeThink = Random.Range(2f, 4f);
        Invoke("think", nextTimeThink);
    }

    void DetectAndMoveTowardsPlayer()
    {
        // �÷��̾ ������ �簢�� ����
        Collider2D[] player = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset, detectionSize, 0f, playerLayer);

        foreach (Collider2D collider in player)
        {
            if (collider.tag == "Player")
            {
                Debug.Log("�÷��̾� ����");
                playerTransform = collider.transform;
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                nextmove = directionToPlayer.x > 0 ? 1 : -1;
            }
        }
    }

    void Dashcoll()
    {
        // �÷��̾ ������ �簢�� ����
        Collider2D[] player1 = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset1, detectionSize1, 0f, playerLayer1);

        foreach (Collider2D collider in player1)
        {
            if (collider.tag == "Player" && !isPreparingToDash)
            {
                move = false;
                dashTarget = collider.transform.position; // �뽬 ��ǥ ����
                dashTarget.y = transform.position.y; // y�� �ڱ�ɷ� ����
                StartCoroutine(PrepareAndDash(dashTarget));
            }
        }
    }



    private IEnumerator PrepareAndDash(Vector2 targetPosition)
    {
        isPreparingToDash = true; // �뽬 �غ� ������ ����
        isDashing = true; // �뽬 ����
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.7f); // �غ� �ð� ���� ���

        dashTarget = targetPosition; // �뽬 ��ǥ ����

        GameObject explosion = Instantiate(dashEffect, this.transform.position, Quaternion.identity);
        Destroy(explosion, 0.5f);

        // �뽬 �� ��ǥ ��ġ�� �̵�
        while (Vector2.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null; // �����Ӹ��� ���

            // �뽬 �߿��� ��������Ʈ ���� ����
            Vector2 direction = (dashTarget - (Vector2)transform.position).normalized;
            spriteRenderer.flipX = direction.x > 0;
        }

        // �뽬 �Ϸ� �� ���� ����
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(cooldownTime); // ��Ÿ�� ���
        isDashing = false; // �뽬 ����
        move = true;
        isPreparingToDash = false; // �뽬 �غ� �Ϸ�
    }

    // �ð������� ���� ���� ǥ�ÿ� (�����)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)detectionOffset, new Vector3(detectionSize.x, detectionSize.y, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + (Vector3)detectionOffset1, new Vector3(detectionSize1.x, detectionSize1.y, 0));
    }
}
