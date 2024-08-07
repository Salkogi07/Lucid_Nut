using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerDamage : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHp player = collision.GetComponent<PlayerHp>();
            player.Damage_HP(damage);
        }
    }
}
