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

        originalGravity = rb.gravityScale; // 원래 중력 값 저장
        rb.gravityScale = 0f; // 대쉬 중 중력 설정

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = originalGravity; // 원래 중력 값으로 복원
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        player.StartPlayerCoroutine(DashStart());

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }

    private IEnumerator DashStart()
    {
        if (!player.isDashing)
        {
            player.isDashing = true;
            player.SetVelocity(player.dashSpeed * player.dashDir, 0);

            while (rb.velocity != Vector2.zero)
            {
                yield return null; // wait for the next frame
            }

            player.isDashing = false;
        }
    }
}
