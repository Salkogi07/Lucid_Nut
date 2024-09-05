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
    public AudioClip attackSound;  // 공격 사운드 클립 추가
    private AudioSource audioSource;  // 오디오 소스 추가

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        audioSource = GetComponent<AudioSource>();  // 오디오 소스 컴포넌트 가져오기

        if (audioSource == null)
        {
            // 만약 오디오 소스가 없으면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (curTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.A) && !playerMove.isAttack)
            {
                audioSource.PlayOneShot(attackSound);  // 공격할 때 사운드 재생
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
                            Debug.Log("최종보스");
                            FinalBossScript boss = collider.GetComponent<FinalBossScript>();
                            boss.BossHp -= playerAttack;
                        }
                        else if (collider.name == "tutorial")
                        {
                            Tutotial_Player_Attack tuto = collider.GetComponent<Tutotial_Player_Attack>();
                            tuto.attackP = true;
                        }

                        EnemyHP enemy = collider.GetComponent<EnemyHP>();

                        if (enemy != null)
                        {
                            enemy.TakeDamage(5);
                        }

                        if(collider.name != "Tornado_boss")
                        {
                            float effectDirection = playerMove.isFacingRight ? 1f : -1f;
                            Vector3 effectScale = new Vector3(effectDirection, 1, 1);
                            GameObject AE = Instantiate(AttackEffect, collider.transform.position, Quaternion.identity);
                            AE.transform.localScale = effectScale;
                            Destroy(AE, 0.5f);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
