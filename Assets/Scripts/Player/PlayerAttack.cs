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
                    if (collider.CompareTag("Enemy") || collider.CompareTag("tornado_Hp"))
                    {
                        if (collider.name == "bossHp(Clone)")
                        {
                            tornado_bHp bossHp = collider.GetComponent<tornado_bHp>();
                            bossHp.attBoss = true;
                        }
                        else if (collider.name == "final_boss")
                        {
                            Debug.Log("��������");
                            FinalBossScript boss = collider.GetComponent<FinalBossScript>();
                            boss.BossHp -= playerAttack;
                        }
                        else if (collider.name == "tutorial")
                        {
                            AttackTuto tuto = collider.GetComponent<AttackTuto>();
                            tuto.attackP = true;
                        }
                        else
                        {
                            EnemyHP enemy = collider.GetComponent<EnemyHP>();
                            if (enemy != null)
                            {
                                enemy.TakeDamage(5);

<<<<<<< HEAD
                                float effectDirection = playerMove.isFacingRight ? 1f : -1f;
                                Vector3 effectScale = new Vector3(effectDirection, 1, 1);
                                GameObject AE = Instantiate(AttackEffect, collider.transform.position, Quaternion.identity);
                                AE.transform.localScale = effectScale;
                                Destroy(AE, 0.5f);
                                Debug.Log(collider.gameObject.name);
                                break;
                            }
=======
                            float effectDirection = playerMove.isFacingRight ? 1f : -1f;
                            Vector3 effectScale = new Vector3(effectDirection, 1, 1);
                            GameObject AE = Instantiate(AttackEffect, collider.transform.position, Quaternion.identity);
                            AE.transform.localScale = effectScale;
                            Destroy(AE, 0.5f);
                            Debug.Log(collider.gameObject.name);
                            break;
>>>>>>> 96abaec83878134ad5a7765f5f141022852703f7
                        }
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

    #region MO
    public void AttackBtn()
    {
        if (!playerMove.isMO && playerMove.isAttack)
            return;

        if (curTime <= 0)
        {
            audioSource.PlayOneShot(attackSound);  // ������ �� ���� ���
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.CompareTag("Enemy"))
                {
                    if (collider.name == "Tornado_boss")
                    {
                        Debug.Log("����̵�����");
                        BossScript boss = collider.GetComponent<BossScript>();
                        boss.BossHp -= playerAttack;
                    }
                    else if (collider.name == "final_boss")
                    {
                        Debug.Log("��������");
                        FinalBossScript boss = collider.GetComponent<FinalBossScript>();
                        boss.BossHp -= playerAttack;
                    }
                    else if (collider.name == "tutorial")
                    {
                        AttackTuto tuto = collider.GetComponent<AttackTuto>();
                        tuto.attackP = true;
                    }
                    else
                    {
                        EnemyHP enemy = collider.GetComponent<EnemyHP>();
                        enemy.TakeDamage(5);

                        float effectDirection = playerMove.isFacingRight ? 1f : -1f;
                        Vector3 effectScale = new Vector3(effectDirection, 1, 1);
                        GameObject AE = Instantiate(AttackEffect, collider.transform.position, Quaternion.identity);
                        AE.transform.localScale = effectScale;
                        Destroy(AE, 0.5f);
                        Debug.Log(collider.gameObject.name);
                        break;
                    }
                }
            }
            playerMove.isAttack = true;
            curTime = coolTime;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
