using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        // 속도에 따른 중력 조절
        if (rb.velocity.y > 0.1f)
        {
            rb.gravityScale = player.gravityScale;
        }
        else if (rb.velocity.y > 0 && rb.velocity.y <= 0.1f)
        {
            rb.gravityScale = player.jumpGravityScale;
        }
        else if (rb.velocity.y < 0)
        {
            if (player.isUmbrellaOpen)
            {
                rb.gravityScale = player.umbrellaFallMultiplier;
            }
            else
            {
                rb.gravityScale = player.fallMultiplier;
            }
        }

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (player.IsGroundDetected())
        {
            player.isJumping = false;
            stateMachine.ChangeState(player.idleState);
        }
    }

}
