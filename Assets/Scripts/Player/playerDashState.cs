using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 대쉬 시작 전에 수평 속도를 0으로 설정
        player.SetVelocity(0, rb.velocity.y);

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

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
        if(!player.isDashing)
        {
            player.isDashing = true;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            player.SetVelocity(player.dashSpeed * player.dashDir, 0);

            while (rb.velocity != Vector2.zero)
            {
                yield return null; // wait for the next frame
            }

            rb.gravityScale = originalGravity;
            player.isDashing = false;
        }
    }
}
