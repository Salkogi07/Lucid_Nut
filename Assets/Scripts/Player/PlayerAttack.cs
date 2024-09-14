using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerMove playerMove;

    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector2 boxSize;
    private int playerAttack = 100;
    public GameObject AttackEffect;
    public AudioClip attackSound;  // ���� ���� Ŭ�� �߰�
    private AudioSource audioSource;  // ����� �ҽ� �߰�

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
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
            if (Input.GetKeyDown(KeyCode.A) && !playerMove.isAttack)
            {
                audioSource.PlayOneShot(attackSound);  // ������ �� ���� ���
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        if (collider.name == "tutorial")
                        {
                            Tutotial_Player_Attack tuto = collider.GetComponent<Tutotial_Player_Attack>();
                            tuto.attackP = true;
                        }

                        EnemyHP enemy = collider.GetComponent<EnemyHP>();

                        if (enemy != null)
                        {
                            enemy.TakeDamage(5);
                        }

                        float effectDirection = playerMove.isFacingRight ? 1f : -1f;
                        Vector3 effectScale = new Vector3(effectDirection, 1, 1);
                        GameObject AE = Instantiate(AttackEffect, collider.transform.position, Quaternion.identity);
                        AE.transform.localScale = effectScale;
                        Destroy(AE, 0.5f);
                    }
                    else if (collider.CompareTag("Boss"))
                    {
                        collider.gameObject.GetComponent<BossHp>().TakeDamage(playerAttack);

                        float effectDirection = playerMove.isFacingRight ? 1f : -1f;
                        Vector3 effectScale = new Vector3(effectDirection, 1, 1);
                        GameObject AE = Instantiate(AttackEffect, collider.transform.position, Quaternion.identity);
                        AE.transform.localScale = effectScale;
                        Destroy(AE, 0.5f);
                    }
                }
                playerMove.isAttack = true;
                curTime = coolTime;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
