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
        player.coyoteTimeCounter = player.coyoteTime; // 코요테 타임 초기화
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.A))
            stateMachine.ChangeState(player.primaryAttack);

        if (!player.IsGroundDetected())
        {
            player.coyoteTimeCounter -= Time.deltaTime;
            if (player.coyoteTimeCounter <= 0)
            {
                stateMachine.ChangeState(player.airState);
            }
        }
        else
        {
            player.coyoteTimeCounter = player.coyoteTime;
        }

        HandleJumpInput();
    }

    private void HandleJumpInput()
    {
        // 위 화살표 키 입력 처리
        if (Input.GetKey(KeyCode.UpArrow))
        {
            player.isChargeJump_inputKey = true;
            player.chargeIndicator.fillAmount = 0f;

            if (player.isUmbrellaOpen && Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            {
                stateMachine.ChangeState(player.chargeJump);
                return;
            }
        }
        else
        {
            player.isChargeJump_inputKey = false;
        }

        // 점프 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Space) && player.CanJump())
        {
            if (player.jumpBufferCounter > 0 || player.coyoteTimeCounter > 0)
            {
                player.jumpBufferCounter = 0;
                player.coyoteTimeCounter = 0;
                stateMachine.ChangeState(player.jumpState);
                return;
            }
        }
    }
}
