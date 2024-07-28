using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerDashState : PlayerState
{
    private float originalGravity;

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.velocity.y);

        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = originalGravity;
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        player.StartCoroutine(DashStart());

        if (stateTimer < 0)
        {
            if (player.IsGroundDetected())
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.airState);
            }
        }
    }

    private IEnumerator DashStart()
    {
        if (!player.isDashing)
        {
            player.isDashing = true;
            player.SetVelocity(player.dashSpeed * player.dashDir, 0);

            while (rb.velocity != Vector2.zero)
            {
                yield return null;
            }

            player.isDashing = false;
        }
    }

}
