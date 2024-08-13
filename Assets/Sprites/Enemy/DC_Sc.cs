using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float cooldownTime = 1f;

    [Header("Detection info")]
    public float detectionRange = 5f; // �÷��̾� ���� ����
    public LayerMask playerLayer; // �÷��̾� ���̾�
    private bool nmove = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
        think();
    }

    void FixedUpdate()
    {
        // �÷��̾� ���� �� ���� ����
        DetectPlayer();

        if (move && !isDashing) // �뽬 ���� �ƴ� ���� �̵�
        {
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);
            // �̵� ���⿡ ���� ��������Ʈ ���� ����
            if (rigid.velocity.x != 0)
            {
                spriteRenderer.flipX = rigid.velocity.x < 0;
            }
        }
        else if (isDashing)
        {
            // �뽬 �߿��� ��������Ʈ ���� ����
            Vector2 direction = (dashTarget - (Vector2)transform.position).normalized;
            spriteRenderer.flipX = direction.x > 0;
        }
    }

    void DetectPlayer()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D rayHitLeft = Physics2D.Raycast(transform.position, Vector2.left, detectionRange, playerLayer);
        RaycastHit2D rayHitRight = Physics2D.Raycast(transform.position, Vector2.right, detectionRange, playerLayer);
        if (rayHitLeft.collider != null && rayHitLeft.collider.CompareTag("Player"))
        {
            Debug.Log("��¿");
            nmove = true;
            nextmove = -1; // �������� �̵�
        }
        else if (rayHitRight.collider != null && rayHitRight.collider.CompareTag("Player"))
        {
            nmove = true;
            nextmove = 1; // ���������� �̵�
        }
        else
        {
            nmove = false;
        }

        // ��������Ʈ ���� ����
        if (nextmove != 0)
        {
            spriteRenderer.flipX = nextmove < 0;
        }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !isPreparingToDash)
        {
            move = false;
            dashTarget = other.transform.position; // �뽬 ��ǥ ����
            dashTarget.y = transform.position.y; // y�� �ڱ�ɷ� ����

            // �뽬 ���� �Ǻ� �� ��������Ʈ ���� ����
            Vector2 direction = (dashTarget - (Vector2)transform.position).normalized;
            spriteRenderer.flipX = direction.x < 0;

            // �뽬 ����
            animator.SetBool("Attack", true);
            StartCoroutine(PrepareAndDash(dashTarget));
        }
        if (other.CompareTag("wall"))
        {
            nextmove *= -1;
            CancelInvoke();
        }
    }

    private IEnumerator PrepareAndDash(Vector2 targetPosition)
    {
        isPreparingToDash = true; // �뽬 �غ� ������ ����
        yield return new WaitForSeconds(0.7f); // �غ� �ð� ���� ���

        dashTarget = targetPosition; // �뽬 ��ǥ ����
        isPreparingToDash = false; // �뽬 �غ� �Ϸ�
        isDashing = true; // �뽬 ����

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
        isDashing = false; // �뽬 ����
        yield return new WaitForSeconds(cooldownTime); // ��Ÿ�� ���
        move = true;
    }
}
