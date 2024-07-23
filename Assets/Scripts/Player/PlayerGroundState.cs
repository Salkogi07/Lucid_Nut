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

        // Prevent normal jump if charge jump is active
        if (player.isChargeJump)
        {
            return;
        }

        if (player.isUmbrellaOpen && player.IsGroundDetected())
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                player.isChargeJump_inputKey = true;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //stateMachine.ChangeState(player.chargeJump);
                    return;
                }
            }
            player.chargeIndicator.fillAmount = 0f;
        }

        if (!player.isChargeJump && Input.GetKeyDown(KeyCode.Space) && player.CanJump())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }
}
