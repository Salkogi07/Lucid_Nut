using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.gravityScale = player.gravityScale;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow) && !player.isChargeJump)
        {
            player.isChargeJump_inputKey = true;
            if (player.isUmbrellaOpen && player.IsGroundDetected())
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    stateMachine.ChangeState(player.chargeJump);
                    return;
                }
            }
            player.chargeIndicator.fillAmount = 0f;
        }
        else
        {
            if (!player.isChargeJump && !Input.GetKey(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.Space) && player.CanJump())
            {
                stateMachine.ChangeState(player.jumpState);
            }
        }
    }
}
