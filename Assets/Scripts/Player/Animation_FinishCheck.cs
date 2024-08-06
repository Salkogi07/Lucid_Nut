using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_FinishCheck : MonoBehaviour
{
    PlayerMove playerMove;

    private void Awake()
    {
        playerMove = GetComponentInParent<PlayerMove>();
    }

    public void OnAttackAnimationEnd()
    {
        playerMove.isAttack = false;
    }
}
