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

        // �߷� ����
        if (rb.velocity.y > 0)
            rb.gravityScale = player.gravityScale; // ��� ���� �� �⺻ �߷� ����
        else if (player.isUmbrellaOpen)
            rb.gravityScale = player.umbrellaFallMultiplier; // ����� ���� �ְ�, �ϰ� ���� �� ��� �߷� ����
        else
            rb.gravityScale = player.gravityScale; // �⺻ �߷� ����

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
