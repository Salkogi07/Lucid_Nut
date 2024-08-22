using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class golem_M : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer; // SpriteRenderer �߰�

    [Header("Move info")]
    private Rigidbody2D rigid;
    public bool move = true;
    public float moveSpeed = 3f;
    public int nextmove;

    [Header("Attack info")]
    public Vector2 detectionSize1 = new Vector2(10f, 2f); // �÷��̾� ���� ���� (�簢�� ũ��)
    public Vector2 detectionOffset1 = Vector2.zero; // �簢���� ��ġ ������
    public LayerMask playerLayer1; // �÷��̾� ���̾�
    public bool attackC = true;

    [Header("Detection info")]
    public Vector2 detectionSize = new Vector2(22f, 2f); // �÷��̾� ���� ���� (�簢�� ũ��)
    public Vector2 detectionOffset = Vector2.zero; // �簢���� ��ġ ������
    public LayerMask playerLayer; // �÷��̾� ���̾�
    public bool nmove = false;

    private Transform playerTransform;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
        think();
    }

    private void Update()
    {
        Attack();
        DetectAndMoveTowardsPlayer();
        if (move)
        {
            moveSpeed = 3f;
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);
            spriteRenderer.flipX = rigid.velocity.x > 0;
        }
    }

    void think()
    {
        if (nmove)
        {
            nextmove = Random.Range(-1, 2);
            if (nextmove == 0)
            {
                animator.SetBool("move", false);
            }
            else
            {
                animator.SetBool("move", true);
            }
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
            if (collider.tag == "Player" && attackC)
            {
                playerTransform = collider.transform;
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                nextmove = directionToPlayer.x > 0 ? 1 : -1;
            }
        }
    }

    void Attack()
    {
        // �÷��̾ ������ �簢�� ����
        Collider2D[] players = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset1, detectionSize1, 0f, playerLayer1);

        foreach (Collider2D collider in players)
        {
            if (collider.tag == "Player" && attackC)
            {
                animator.SetBool("move", true);
                attackC = false;
                StartCoroutine(FireProjectile(collider.transform.position));
                break; // ������ �÷��̾ ������ �� ���� �߻�
            }
        }
    }

    private IEnumerator FireProjectile(Vector2 targetPosition)
    {
        Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;
        nextmove = directionToPlayer.x > 0 ? 1 : -1;
        spriteRenderer.flipX = nextmove > 0;

        // ��ų ��� �� �̵��� ���߰� �ִϸ��̼� ����
        nextmove = 0;
        nmove = false;
        move = false;
        moveSpeed = 0f;
        animator.SetBool("Attack", true);
        animator.SetBool("move", false);
        yield return new WaitForSeconds(1f);


        animator.SetBool("Attack", false);
        animator.SetBool("move", true);
        yield return new WaitForSeconds(1f);
        nmove = true;
        nextmove = Random.Range(-1, 2);
        move = true;
        attackC = true;
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
