using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
        rb.gravityScale = player.gravityScale;
        player.ResetJumpBuffer();
        player.ResetCoyoteTime();
        player.isJumping = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.isJumping = false;
    }

    public override void Update()
    {
        base.Update();

        if (player.isChargeJump)
            return;

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);
    }
}
