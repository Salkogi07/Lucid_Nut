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

        // 중력 설정
        if (rb.velocity.y > 0)
            rb.gravityScale = player.gravityScale; // 상승 중일 때 기본 중력 유지
        else if (player.isUmbrellaOpen)
            rb.gravityScale = player.umbrellaFallMultiplier; // 우산이 열려 있고, 하강 중일 때 우산 중력 적용
        else
            rb.gravityScale = player.gravityScale; // 기본 중력 적용

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
