using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerMove playerMove;

    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector2 boxSize;
    private int playerAttack = 100;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (curTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        if (collider.name == "Tornado_boss")
                        {
                            Debug.Log("토네이도보스");
                            BossScript boss = collider.GetComponent<BossScript>();
                            boss.BossHp -= playerAttack;
                        }
                        else if (collider.name == "final_boss")
                        {
                            Debug.Log("최종보스");
                            FinalBossScript boss = collider.GetComponent<FinalBossScript>();
                            boss.BossHp -= playerAttack;
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
