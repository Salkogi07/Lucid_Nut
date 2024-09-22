using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHP enemy = collision.GetComponent<EnemyHP>();
            if (enemy != null)
                enemy.TakeDamage(20f);  // 데미지 적용
        }
        else if (collision.CompareTag("Boss"))
        {
            BossHp boss = collision.GetComponent<BossHp>();
            if (boss != null)
                boss.TakeDamage(20);  // 데미지 적용
        }
    }
}
